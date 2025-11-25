using OfficeOpenXml; //fungsi excel(dari nuget)
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SpectrumAnalyzerGUI
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private CancellationTokenSource measureCts;
        private bool isConnected = false;
        private bool isMeasuring = false;
        private string connectedDeviceName = "Unknown Device";
        private int lastPointCount = 1000;
        
        private double[] lastFreqsMHz;
        private float[] lastAmplitudes;

        private readonly object dataLock = new object();
        private bool isFormClosing = false;

        public Form1()
        {
            InitializeComponent();

            chart1.ChartAreas.Add("area");
            chart1.Series.Clear();
            chart1.Series.Add("Trace");
            chart1.Series["Trace"].ChartType = SeriesChartType.Line;
            chart1.Series["Trace"].IsXValueIndexed = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isFormClosing = true;
            try { measureCts?.Cancel(); } catch { }
            try { stream?.Close(); client?.Close(); } catch { }
            base.OnFormClosing(e);
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        { 
            try
            {
                string ip = TxtResource.Text;
                int port = 5025;
                using (var cts = new CancellationTokenSource(3000)) //timeout 3 dtk
                {
                    client = new TcpClient();
                    var connectTask = client.ConnectAsync(ip, port);
                    var completedTask = await Task.WhenAny(connectTask, Task.Delay(Timeout.Infinite, cts.Token));
                    if (completedTask != connectTask)
                    {
                        throw new TimeoutException("Connection timed out.");
                    }
                    stream = client.GetStream();
                    stream.ReadTimeout = 3000;
                    stream.WriteTimeout = 3000;
                    isConnected = true;
                }
                connectedDeviceName = await GetDeviceNameAsync();
                BtnConnect.Enabled = false;
                BtnDisconnect.Enabled = true;
                LblName.Text = $"Device name: {connectedDeviceName}";
                SafeBeginInvoke(async () =>
                {
                    await MessageBox.ShowAsync(
                        $"Connected to {connectedDeviceName}",
                        "Info",
                        MessageBox.ToastIcon.Info,
                        1500,
                        this
                    );
                });
            }
            catch (Exception ex)
            {
                ShowError("Failed to connect: ", ex); 
                BtnConnect.Enabled = true;
                BtnDisconnect.Enabled = false;
            }
        }

        private async Task<string> GetDeviceNameAsync()
        {
            if (stream == null || !client.Connected)
                return "Unknown Device";
            try
            {
                byte[] cmd = Encoding.ASCII.GetBytes("*IDN?\n");
                await stream.WriteAsync(cmd, 0, cmd.Length);

                byte[] buffer = new byte[512];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                if (string.IsNullOrWhiteSpace(response))
                    return "Unknown Device";

                string[] parts = response.Split(',');
                if(parts.Length >= 2)
                {
                    string manufacturer = parts[0].Trim();
                    string model = parts[1].Trim();
                    return $"{manufacturer}, {model}";
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IDN ERROR] {ex.Message}");
                return "Unknown Device";
            }
        }

        private void SendCommand(string cmd)
        {
            if (stream == null) return;
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(cmd + "\n");
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                ShowError("SendCommand error: ", ex);
            }
        }

        private async Task<String> ReadStringResponseAsync()
        {
            if (stream == null) return "";
            try
            {
                byte[] buffer = new byte[16384];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
            }
            catch (Exception ex)
            {
                ShowError("Read error: ", ex);
                return "";
            }
        }

        private async Task<byte[]> ReadBinaryResponseAsync()
        {
            if (stream == null) return new byte[0];
            if (!client.Connected) return new byte[0];
            List<byte> allBytes = new List<byte>();
            byte[] buffer = new byte[8192];
            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    for (int i = 0; i < bytesRead; i++) allBytes.Add(buffer[i]);
                    if (allBytes.Count > 0 && allBytes.Last() == 0x0A) break;
                }
            }
            catch (Exception ex)
            {
                ShowError("ReadBinary error: ", ex);
            }
            return allBytes.ToArray();
        }

        private string SafeString(object val, string format = null)
        {
            if (val == null) return "0";
            if (val is double d) return d.ToString(format ?? "G", CultureInfo.InvariantCulture);
            if (val is float f) return f.ToString(format ?? "G", CultureInfo.InvariantCulture);
            return val.ToString();
        }

        private void SafeBeginInvoke(Action action)
        {
            try
            {
                if (this.IsDisposed || !this.IsHandleCreated || isFormClosing) return;
                this.BeginInvoke(action);
            }
            catch { }
        }

        private async Task DoMeasure()
        {
            if (stream == null || !client.Connected)
            {
                await MessageBox.ShowAsync("Not connected.", "Warning",
                    MessageBox.ToastIcon.Warning, 1500, this);
                return;
            }
            try
            {   
                if (isFormClosing || client == null || !client.Connected) return;

                SendCommand(":FREQ:STAR?");
                double.TryParse(await ReadStringResponseAsync(), NumberStyles.Float, CultureInfo.InvariantCulture, out double startFreqHz);

                SendCommand(":FREQ:STOP?");
                double.TryParse(await ReadStringResponseAsync(), NumberStyles.Float, CultureInfo.InvariantCulture, out double stopFreqHz);

                SendCommand(":FREQ:CENT?");
                double.TryParse(await ReadStringResponseAsync(), NumberStyles.Float, CultureInfo.InvariantCulture, out double centFreqHz);

                SendCommand(":FREQ:SPAN?");
                double.TryParse(await ReadStringResponseAsync(), NumberStyles.Float, CultureInfo.InvariantCulture, out double spanHz);

                SafeBeginInvoke(() =>
                {
                    TxtCentFreq.Text = (centFreqHz / 1e6).ToString("F1");
                    TxtSpan.Text = (spanHz / 1e6).ToString("F1");
                });

                SendCommand(":FORM:DATA REAL32");
                SendCommand(":TRAC:DATA? TRACE1");
                byte[] rawData = await ReadBinaryResponseAsync();
                if (rawData.Length > 0 && rawData[rawData.Length - 1] == 0x0A)
                    Array.Resize(ref rawData, rawData.Length - 1);

                int totalPoints = rawData.Length / 4;
                if (totalPoints <= 0) return;
                lastPointCount = totalPoints;

                float[] amplitudes = new float[totalPoints];
                for (int i = 0; i < totalPoints; i++)
                {
                    float val = BitConverter.ToSingle(rawData, i * 4);
                    if (float.IsNaN(val) || float.IsInfinity(val)) val = 0f;
                    amplitudes[i] = val;
                }

                double[] freqsMHz = new double[totalPoints];
                double stepHz = (totalPoints > 1) ? (stopFreqHz - startFreqHz) / (totalPoints - 1) : 0;
                for (int i = 0; i < totalPoints; i++)
                    freqsMHz[i] = (startFreqHz + i * stepHz) / 1e6;
                
                lock (dataLock)
                {
                    lastFreqsMHz = freqsMHz;
                    lastAmplitudes = amplitudes;
                }

                float peakAmp = amplitudes.Max();
                int peakIndex = Array.IndexOf(amplitudes, peakAmp);
                double peakFreq = (peakIndex >= 0 && peakIndex < freqsMHz.Length) ? freqsMHz[peakIndex] : 0;

                SafeBeginInvoke(() =>
                {
                    LblStart.Text = $"Start Freq: {startFreqHz / 1e6:F3} MHz";
                    LblStop.Text = $"Stop Freq: {stopFreqHz / 1e6:F3} MHz";
                    LblPeakFreq.Text = $"Peak Freq: {peakFreq:F3} MHz";
                    LblPeakAmp.Text = $"Peak Amp: {peakAmp:F2} dBm";
                    
                    var series = chart1.Series["Trace"];
                    var area = chart1.ChartAreas[0];
                    area.AxisX.Minimum = freqsMHz.First();
                    area.AxisX.Maximum = freqsMHz.Last();
                    area.RecalculateAxesScale();

                    double[] ampsDouble = amplitudes.Select(a => (double)a).ToArray();
                    series.Points.Clear();
                    series.Points.DataBindXY(freqsMHz, ampsDouble);
                });
            }
            catch (Exception ex)
            {
                ShowError("Error during measure: ", ex);
            }
        }

        private int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            TxtCentFreq.Enabled = false;
            TxtSpan.Enabled = false;
            BtnApply.Enabled = false;
            if (client == null || !isConnected)
            {
                await MessageBox.ShowAsync("Not connected.", "Warning",
                    MessageBox.ToastIcon.Warning, 1200, this);
                return;
            }
            if (isMeasuring) return;
            isMeasuring = true;
            measureCts = new CancellationTokenSource();
            try
            {
                await Task.Run(async () =>
                {
                    while (!measureCts.Token.IsCancellationRequested && !isFormClosing)
                    {
                        if (this.IsDisposed) break;
                        await DoMeasure();
                        await Task.Delay(100, measureCts.Token);
                    }
                }, measureCts.Token);
            }
            catch {}
            finally { isMeasuring = false; }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (client == null || !isConnected)
                {
                    _ = MessageBox.ShowAsync("Not connected.", "Warning",
                        MessageBox.ToastIcon.Warning, 1200, this);
                    return;
                }
                TxtCentFreq.Enabled = true;
                TxtSpan.Enabled = true;
                BtnApply.Enabled = true;
                if (isMeasuring && measureCts != null) measureCts.Cancel();
            }
            catch (Exception ex)
            {
                ShowError("error: ", ex);
            }
        }

        private async void BtnApply_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(TxtCentFreq.Text, out double centerMHz) ||
                !double.TryParse(TxtSpan.Text, out double spanMHz))
            {
                await MessageBox.ShowAsync("Invalid input.", "Error",
                    MessageBox.ToastIcon.Error, 1500, this);
                return;
            }
            SendCommand($":FREQ:CENT {centerMHz * 1e6}");
            SendCommand($":FREQ:SPAN {spanMHz * 1e6}");
        }

        private async void BtnRunTest_Click(object sender, EventArgs e)
        {
            if (client == null || !isConnected)
            {
                await MessageBox.ShowAsync("Not connected.", "Warning",
                    MessageBox.ToastIcon.Warning, 1200, this);
                return;
            }
            if (dataGridView1.Rows.Count <= 1)
            {
                await MessageBox.ShowAsync("No data.", "Warning",
                    MessageBox.ToastIcon.Warning, 1200, this);
                return;
            }

            if (isMeasuring) { measureCts?.Cancel(); await Task.Delay(300); }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (isFormClosing) break;
                if (row.IsNewRow) continue;

                double.TryParse(row.Cells["Freq"].Value?.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double freqMHz);
                double.TryParse(row.Cells["Limit"].Value?.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double limitDbm);

                SendCommand($":FREQ:CENT {freqMHz * 1e6}");
                int adaptiveDelay = Clamp((int)(lastPointCount * 0.1), 250, 750);
                await Task.Delay(adaptiveDelay);
                await Task.Run(() => DoMeasure());

                double[] freqsCopy;
                float[] ampsCopy;
                lock (dataLock)
                {
                    freqsCopy = lastFreqsMHz?.ToArray() ?? Array.Empty<double>();
                    ampsCopy = lastAmplitudes?.ToArray() ?? Array.Empty<float>();
                }

                if (freqsCopy.Length == 0)
                {
                    row.Cells["Power"].Value = "0.00";
                    row.Cells["Status"].Value = "NO TRACE";
                    continue;
                }

                int nearestIdx = 0;
                double minDiff = double.MaxValue;
                for (int i = 0; i < freqsCopy.Length; i++)
                {
                    double diff = Math.Abs(freqsCopy[i] - freqMHz);
                    if (diff < minDiff) { minDiff = diff; nearestIdx = i; }
                }

                float amp = ampsCopy[nearestIdx];
                row.Cells["Power"].Value = SafeString(amp, "F2");
                row.Cells["Freq"].Value = SafeString(freqMHz, "F2");
                row.Cells["Freq"].ToolTipText = $"{freqsCopy[nearestIdx]:F3} MHz (actual)";
                row.Cells["Limit"].Value = SafeString(limitDbm, "F2");
                string status = (amp >= limitDbm) ? "PASS" : "FAIL";
                row.Cells["Status"].Value = status;
                row.DefaultCellStyle.BackColor = (status == "PASS") ? Color.LightGreen : Color.LightPink;
            }
            SafeBeginInvoke(async () =>
            {
                await MessageBox.ShowAsync("Test selesai.", "Done",
                    MessageBox.ToastIcon.Success, 1500, this);
            });
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            try { if (isMeasuring && measureCts != null) measureCts.Cancel(); } catch { }
            try { stream?.Close(); client?.Close(); } catch { }
            Close();
        }

        private DataSet1 GetDataSet()
        {
            DataSet1 ds = new DataSet1();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    ds.DataTable1.AddDataTable1Row(
                        Convert.ToInt32(row.Cells["No"].Value ?? 0),
                        row.Cells["Freq"].Value?.ToString(),
                        row.Cells["Limit"].Value?.ToString(),
                        row.Cells["Power"].Value?.ToString(),
                        row.Cells["Status"].Value?.ToString(),
                        connectedDeviceName
                    );
                }
            }
            return ds;
        }
        
        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelPackage.License.SetNonCommercialOrganization("TEST");
                using (OpenFileDialog ofd = new OpenFileDialog()
                { Filter = "Excel files (*.xlsx)|*.xlsx" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (var package = new ExcelPackage(new FileInfo(ofd.FileName)))
                        {
                            ExcelWorksheet ws = package.Workbook.Worksheets.First();
                            dataGridView1.Rows.Clear();
                            dataGridView1.Columns.Clear();

                            dataGridView1.Columns.Add("No", "No");
                            dataGridView1.Columns.Add("Freq", "Freq (MHz)");
                            dataGridView1.Columns.Add("Limit", "Limit (dBm)");
                            dataGridView1.Columns.Add("Power", "Power (dBm)");
                            dataGridView1.Columns.Add("Status", "Status");

                            int row = 2;
                            while (ws.Cells[row, 1].Value != null)
                            {
                                string noStr = ws.Cells[row, 1].Text;
                                string freqStr = ws.Cells[row, 2].Text;
                                string limitStr = ws.Cells[row, 3].Text;

                                if (!int.TryParse(noStr, out int no))
                                    throw new Exception($"Row {row}: Kolom 'No' harus angka integer!");

                                if (!double.TryParse(freqStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double freq))
                                    throw new Exception($"Row {row}: Kolom 'Freq' harus angka valid!");

                                if (!double.TryParse(limitStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double limit))
                                    throw new Exception($"Row {row}: Kolom 'Limit' harus angka valid!");

                                dataGridView1.Rows.Add(no, freq, limit, "", "");
                                row++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Import error: ", ex);
            }
        }

        private void ShowError(string message, Exception ex = null)
        {
            try
            {
                string fullMessage = $"{message}";
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    fullMessage += $"\n{ex.Message}";

                if (this.IsDisposed || isFormClosing)
                {
                    Console.WriteLine($"[Error Handler] {fullMessage}");
                    return;
                }

                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        _ = MessageBox.ShowAsync(
                            fullMessage,
                            "Error",
                            MessageBox.ToastIcon.Error,
                            2000,
                            this
                        );
                    }));
                }
                else
                {
                    _ = MessageBox.ShowAsync(
                        fullMessage,
                        "Error",
                        MessageBox.ToastIcon.Error,
                        2500,
                        this
                    );
                }
            }
            catch
            {
                Console.WriteLine($"[Error Handler] {message}: {ex?.Message}");
            }
        }


        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (isMeasuring && measureCts != null)
                {
                    measureCts.Cancel();
                    Task.Delay(300).Wait();
                }
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
                if (client != null)
                {
                    client.Close();
                    client = null;
                }
                isConnected = false;
                isMeasuring = false;
                BtnApply.Enabled = false;
                connectedDeviceName = "Unknown Device";

                SafeBeginInvoke(() =>
                {
                    TxtCentFreq.Enabled = false;
                    TxtSpan.Enabled = false;
                    LblName.Text = "Device name: -";
                    chart1.Series["Trace"].Points.Clear();
                    TxtCentFreq.Clear();
                    TxtSpan.Clear();
                    LblStart.Text = "Start Freq: - MHz";
                    LblStop.Text = "Stop Freq: - MHz";
                    LblPeakFreq.Text = "Peak Freq: - MHz";
                    LblPeakAmp.Text = "Peak Amp: - dBm";
                    _ = MessageBox.ShowAsync("Disconnected.", "Info",
                        MessageBox.ToastIcon.Info, 1200, this);
                });
            }
            catch (Exception ex)
            {
                ShowError("Disconnect error: ", ex);
            }
            finally
            {
                BtnConnect.Enabled = true;
                BtnDisconnect.Enabled = false;
            }

        }

        private async void BtnClear_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                await MessageBox.ShowAsync("No data to clear.", "Warning",
                    MessageBox.ToastIcon.Warning, 1200, this);
                return;
            }
            try
            {
                dataGridView1.Rows.Clear();
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.DefaultCellStyle.BackColor = Color.White;
                    col.DefaultCellStyle.ForeColor = Color.Black;
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }

            }
            catch (Exception ex)
            {
                ShowError("Clear error: ", ex);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataSet1 ds = GetDataSet();
            int passcount = ds.DataTable1.Count(r => r.Status == "PASS");
            int failcount = ds.DataTable1.Count(r => r.Status == "FAIL");
            int totalcount = passcount + failcount;
            double percentagepass = (totalcount > 0) ? (passcount * 100.0 / totalcount) : 0.00;

            ReportForm rf = new ReportForm(ds, connectedDeviceName, passcount, failcount, percentagepass);
            rf.ShowDialog();
        }
    }
}