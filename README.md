# Description
Proyek ini adalah aplikasi Automated Test Equipment (ATE) yang dirancang untuk mengotomatisasi proses pengukuran, monitoring, dan pengujian pada perangkat Spectrum Analyzer, khusunya untuk **Siglent SSA3021X Plus** & **Siglent SHA851A**.<br/> 

Aplikasi ini dibuat menggunakan **C# Windows Forms** dan mencakup komponen UI kustom seperti visualisasi grafik real-time, notifikasi khusus, integrasi Excel, serta Crystal Reports untuk pembuatan laporan otomatis.<br/>

Aplikasi ini menggunakan **SCPI (Standard Commands for Programmable Instruments)** melalui TCP/IP untuk berkomunikasi dengan analyzer, mengambil data trace, dan melakukan pengujian frekuensi otomatis dengan evaluasi PASS/FAIL.
# Installation
### Requirements
- Windows 10/11
- .NET Desktop Runtime 
- Visual Studio 2022 (for option 2)
- Crystal Report runtime (di releases v1.1.0 atau [releases](https://github.com/Fitznug045/RF-Spectrum-ATE/releases/tag/v.1.1.0))

## Option 1 : Install prebuilt application
- Download dari halaman [releases](https://github.com/Fitznug045/RF-Spectrum-ATE/releases/tag/v.1.1.0) and ekstrak file ZIP
- Buka folder Release dan jalankan SpectrumAnalyzerGUI.exe

## Option 2 : Build from source code
- Clone repository
```
git clone https://github.com/Fitznug045/RF-Spectrum-ATE.git
cd RF-Spectrum-ATE
```
- Buka file solution (.sln) di Visual Studio 2022
- Install NuGet packages:<br/>
  1. EPPlus (Excel reading)
  2. OpenTK
  3. System.Data.DataSetExtensions
- Ubah konfigurasi dari **Debug** ke **Release**
- Pastikan Crystal Reports Runtime terinstal
- Build project :
```
Build -> Build Solution
```
- Executible akan terletak di :
```
/bin/Release/
```

# User Guide
### 1. Connecting to Spectrum Analyzer
- Buka aplikasi dan nyalakan Spectrum Analyzer
- Pastikan instrumen terkoneksi ke jaringan yang sama dengan PC
- Ambil alamat IP analyzer dan masukkan ke kolom **Resource Address**
- Klik **Connect**<br/>

Jika koneksi berhasil :
- Nama perangkat akan tampil
- Notifikasi akan muncul
- Kontrol pengukuran akan aktif

### 2. Starting Measurement
- Setelah terhubung, klik **Start** untuk melakukan pengukuran
- Grafik akan diperbarui secara real-time
- Nilai berikut akan terupdate otomatis :<br/>
  - Start Frequency
  - Stop Frequency
  - Peak Frequency
  - Peak Amplitude
- Untuk menghentikan pengukuran, klik **Stop**

### 3. Adjusting Frequency Settings
Anda dapat mengatur center frequency dan span secara manual dengan:<br/>
1. klik **Stop**
2. input nilai di center frequency dan span
3. klik **Apply**

### 4. Importing Test List
Fitur ini memungkinkan pengujian otomatis PASS/FAIL menggunakan daftar frekuensi dari Excel.<br/>
**Excel Format**<br/>
| No | Freq (MHz) | Limit (dBm) |
| -- | ---------- | ----------- |
| 1  | 100        | -30         |
| 2  | 150        | -28         |

Untuk melakukan import :
1. Klik **Import**
2. pilih file .xlsx
3. Data excel data akan dimasukkan ke tabel

### 5. Running Automated Test (Batch Test)
Setelah melakukan import:<br/>
1. Pastikan analyzer terhubung
2. Klik **Run Test**

Untuk setiap frekuensi :<br/>
- Analyzer akan dituning otomatis
- Trace akan terambil
- Sampel frekuensi dievaluasi
- PASS/FAIL dihitung
- Baris tabel diberi warna :
  - Hijau = PASS
  - Merah = FAIL<br/>
- Notifikasi akan muncul setelah tes selesai.

### 6. Saving Results & Creating Reports
Setelah melakukan pengujian, klik **Save**.

Laporan berbentuk PDF akan dihasilkan menggunakan Crystal Reports, berisi :<br/>
- Device Name
- Test Date & Time
- PASS/FAIL Summary
- Pass Percentage
- Full test table

Di report viewer anda bisa :
- Export to PDF
- Print
- Save to file

### 7. Clearing the Table
Klik **Clear** untuk menghapus semua baris dari tabel pengujian.<br/>
Tindakan ini tidak akan mempengaruhi grafik atau status koneksi.

### 8. Disconnecting
Untuk memutuskan koneksi dengan aman:
- Klik **Disconnect**
- Loop pengukuran akan berhenti
- UI akan kembali ke keadaan idle
