using System;
using System.Collections.Generic;


// 1. 

class Sekolah
{
    public string IdSekolah { get; set; }
    public string NamaSekolah { get; set; }
    public int JumlahPaketDiterima { get; private set; }

    public Sekolah(string id, string nama)
    {
        IdSekolah = id;
        NamaSekolah = nama;
        JumlahPaketDiterima = 0;
    }

    // Menambahkan jumlah paket yang diterima sekolah
    public void TambahPaketDiterima(int jumlah)
    {
        if (jumlah > 0)
        {
            JumlahPaketDiterima += jumlah;
        }
    }

    // Menampilkan informasi sekolah
    public void TampilInfo()
    {
        Console.WriteLine("ID Sekolah       : " + IdSekolah);
        Console.WriteLine("Nama Sekolah     : " + NamaSekolah);
        Console.WriteLine("Paket Diterima   : " + JumlahPaketDiterima);
    }
}


// 2. 

abstract class PaketMBG
{
    private string idPaket;
    private string namaPaket;
    private int stokDapur;

    public string IdPaket
    {
        get { return idPaket; }
        set { idPaket = value; }
    }

    public string NamaPaket
    {
        get { return namaPaket; }
        set { namaPaket = value; }
    }

    // Stok hanya bisa dibaca dari luar class
    public int StokDapur
    {
        get { return stokDapur; }
    }

    public PaketMBG(string id, string nama, int stok)
    {
        if (stok < 0)
            throw new ArgumentException("Stok tidak boleh negatif!");

        idPaket = id;
        namaPaket = nama;
        stokDapur = stok;
    }

    // Method khusus untuk mengurangi stok
    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0 || jumlah > stokDapur)
        {
            return false;
        }

        stokDapur -= jumlah;
        return true;
    }

    // Method khusus untuk menambahkan stok
    public void TambahStok(int jumlah)
    {
        if (jumlah > 0)
        {
            stokDapur += jumlah;
        }
    }

    // Abstract method untuk menghitung biaya
    public abstract decimal HitungBiaya(int jumlah);
}


// 3. 

class PaketReguler : PaketMBG
{
    public PaketReguler(string id, string nama, int stok)
        : base(id, nama, stok)
    {
    }

    // Menghitung biaya paket reguler
    public override decimal HitungBiaya(int jumlah)
    {
        return jumlah * 15000;
    }
}

// ==========================================
// 3. INHERITANCE - PAKET KHUSUS
// ==========================================
class PaketKhusus : PaketMBG
{
    public string JenisDiet { get; set; }

    public PaketKhusus(string id, string nama, int stok, string diet)
        : base(id, nama, stok)
    {
        JenisDiet = diet;
    }

    // Menghitung biaya paket khusus
    public override decimal HitungBiaya(int jumlah)
    {
        return jumlah * 20000;
    }
}

// 4. INTERFACE

interface DapatDidistribusikan
{
    void KirimPaket();
}

// CLASS DISTRIBUSI MBG

class DistribusiMBG : DapatDidistribusikan
{
    private Sekolah sekolahTujuan;
    private PaketMBG paket;
    private int jumlah;

    public DistribusiMBG(
        Sekolah sekolah,
        PaketMBG paketMakanan,
        int jumlahKirim)
    {
        sekolahTujuan = sekolah;
        paket = paketMakanan;
        jumlah = jumlahKirim;
    }

    public void KirimPaket()
    {
        Console.WriteLine("\n--- PROSES DISTRIBUSI MBG ---");

        // Memastikan jumlah pengiriman valid
        if (jumlah <= 0)
        {
            Console.WriteLine("Distribusi gagal! Jumlah paket harus lebih dari 0.");
            return;
        }

        // Memastikan stok mencukupi
        if (jumlah > paket.StokDapur)
        {
            Console.WriteLine("Distribusi gagal! Stok dapur tidak mencukupi.");
            Console.WriteLine("Stok tersedia : " + paket.StokDapur);
            Console.WriteLine("Jumlah diminta : " + jumlah);
            return;
        }

        // Mengurangi stok dapur
        bool berhasil = paket.KurangiStok(jumlah);

        if (berhasil)
        {
            // Menambahkan jumlah paket yang diterima sekolah
            sekolahTujuan.TambahPaketDiterima(jumlah);

            Console.WriteLine(
                "Distribusi MBG berhasil! Sebanyak " +
                jumlah + " paket makanan telah dikirim ke sekolah " +
                sekolahTujuan.NamaSekolah);
        }
        else
        {
            Console.WriteLine("Distribusi gagal! Pengurangan stok tidak berhasil.");
        }
    }
}

// 5. PENGELOLA MBG (OPSIONAL)

class PengelolaMBG
{
    private List<Sekolah> daftarSekolah;
    private List<PaketMBG> daftarPaket;

    public PengelolaMBG()
    {
        daftarSekolah = new List<Sekolah>();
        daftarPaket = new List<PaketMBG>();
    }

    // Aggregation:

    public void TambahSekolah(Sekolah sekolah)
    {
        daftarSekolah.Add(sekolah);
    }

    public void TambahPaket(PaketMBG paket)
    {
        daftarPaket.Add(paket);
    }

    // Menampilkan seluruh data sekolah dan paket
    public void TampilkanDataMBG()
    {
        Console.WriteLine("\n================================");
        Console.WriteLine("       DATA MANAJEMEN MBG");
        Console.WriteLine("================================");

        Console.WriteLine("\nDATA SEKOLAH");

        foreach (Sekolah sekolah in daftarSekolah)
        {
            sekolah.TampilInfo();
            Console.WriteLine("----------------------------");
        }

        Console.WriteLine("\nDATA PAKET MAKANAN");

        foreach (PaketMBG paket in daftarPaket)
        {
            Console.WriteLine("ID Paket    : " + paket.IdPaket);
            Console.WriteLine("Nama Paket  : " + paket.NamaPaket);
            Console.WriteLine("Stok Dapur  : " + paket.StokDapur);
            Console.WriteLine("----------------------------");
        }
    }
}
// 6. PROGRAM UTAMA
// ==========================================
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("====================================");
        Console.WriteLine("   SISTEM MANAJEMEN DISTRIBUSI MBG");
        Console.WriteLine("====================================");

        // Membuat dua objek sekolah
        Sekolah sekolah1 = new Sekolah("S001", "SMA Negeri Kerupuk");
        Sekolah sekolah2 = new Sekolah("S002", "SMA Negeri Peyek");

        // Membuat objek paket makanan
        PaketReguler reguler = new PaketReguler(
            "P088", "Paket Reguler", 100);

        PaketKhusus khusus = new PaketKhusus(
            "P888", "Paket Khusus", 50, "Rendah Gula");

        // ==================================
        // MENAMPILKAN DATA SEKOLAH
        // ==================================
        Console.WriteLine("\n--- DATA SEKOLAH ---");

        sekolah1.TampilInfo();

        Console.WriteLine();

        sekolah2.TampilInfo();

        // ==================================
        // MENAMPILKAN DATA PAKET
        // ==================================
        Console.WriteLine("\n--- DATA PAKET MBG ---");

        Console.WriteLine("Paket : " + reguler.NamaPaket);
        Console.WriteLine("Stok  : " + reguler.StokDapur);

        Console.WriteLine("\nPaket : " + khusus.NamaPaket);
        Console.WriteLine("Diet  : " + khusus.JenisDiet);
        Console.WriteLine("Stok  : " + khusus.StokDapur);

        // ==================================
        // POLYMORPHISM
        // ==================================
        Console.WriteLine("\n--- PERHITUNGAN BIAYA ---");

        PaketMBG paket1 = reguler;
        PaketMBG paket2 = khusus;

        Console.WriteLine("Biaya Paket Reguler (10 paket): Rp" +
            paket1.HitungBiaya(10).ToString("N0"));

        Console.WriteLine("Biaya Paket Khusus (10 paket): Rp" +
            paket2.HitungBiaya(10).ToString("N0"));

        // ==================================
        // PROSES DISTRIBUSI BERHASIL
        // ==================================
        DistribusiMBG distribusi1 = new DistribusiMBG(
            sekolah1, reguler, 30);

        distribusi1.KirimPaket();

        // ==================================
        // PROSES DISTRIBUSI GAGAL
        // ==================================
        DistribusiMBG distribusi2 = new DistribusiMBG(
            sekolah2, khusus, 100);

        distribusi2.KirimPaket();

        // ==================================
        // PENGELOLAAN DATA MBG
        // ==================================
        PengelolaMBG pengelola = new PengelolaMBG();

        pengelola.TambahSekolah(sekolah1);
        pengelola.TambahSekolah(sekolah2);

        pengelola.TambahPaket(reguler);
        pengelola.TambahPaket(khusus);

        pengelola.TampilkanDataMBG();

        // ==================================
        // DATA AKHIR DISTRIBUSI
        // ==================================
        Console.WriteLine("\n--- DATA AKHIR DISTRIBUSI ---");

        sekolah1.TampilInfo();

        Console.WriteLine("\nSisa Stok Paket Reguler : " +
            reguler.StokDapur);

        Console.WriteLine("Sisa Stok Paket Khusus  : " +
            khusus.StokDapur);

        Console.WriteLine("\nProgram selesai dijalankan.");
        Console.ReadKey();
    }
}