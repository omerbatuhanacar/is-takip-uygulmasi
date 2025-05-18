using System;
using System.Collections.Generic;
using System.Text;

namespace IsTakipYonetimSistemi
{
    public class Calisan
    {
        public int CalisanId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public List<Gorev> Gorevler { get; set; }

        public Calisan(int id, string ad, string soyad)
        {
            CalisanId = id;
            Ad = ad;
            Soyad = soyad;
            Gorevler = new List<Gorev>();
        }

        public override string ToString()
        {
            return $"Çalışan ID: {CalisanId}, Adı: {Ad} {Soyad}";
        }
    }
    public class Gorev
    {
        public int GorevId { get; set; }
        public string GorevAdi { get; set; }
        public Calisan Sorumlu { get; set; }
        public int Ilerleme { get; set; }

        public Gorev(int id, string gorevAdi)
        {
            GorevId = id;
            GorevAdi = gorevAdi;
            Ilerleme = 0;
            Sorumlu = null;
        }

        public void IlerlemeKaydet(int yeniIlerleme)
        {
            if (yeniIlerleme < 0 || yeniIlerleme > 100)
            {
                Console.WriteLine("İlerleme değeri 0 ile 100 arasında olmalıdır.");
                return;
            }
            Ilerleme = yeniIlerleme;
        }

        public override string ToString()
        {
            string sorumluAdi = Sorumlu == null ? "Atanmamış" : Sorumlu.Ad + " " + Sorumlu.Soyad;
            return $"Görev ID: {GorevId}, Adı: {GorevAdi}, Sorumlu: {sorumluAdi}, İlerleme: %{Ilerleme}";
        }
    }


    public class Proje
    {
        public int ProjeId { get; set; }
        public string ProjeAdi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public List<Gorev> Gorevler { get; set; }

        public Proje(int id, string projeAdi, DateTime baslangic, DateTime bitis)
        {
            ProjeId = id;
            ProjeAdi = projeAdi;
            BaslangicTarihi = baslangic;
            BitisTarihi = bitis;
            Gorevler = new List<Gorev>();
        }

        public void GorevEkle(Gorev gorev)
        {
            Gorevler.Add(gorev);
        }

        public double OrtalamaIlerleme
        {
            get
            {
                if (Gorevler.Count == 0) return 0;
                double toplam = 0;
                foreach (var g in Gorevler)
                    toplam += g.Ilerleme;
                return toplam / Gorevler.Count;
            }
        }

        public override string ToString()
        {
            return $"Proje ID: {ProjeId}, Adı: {ProjeAdi}, Başlangıç: {BaslangicTarihi.ToShortDateString()}, " +
                   $"Bitiş: {BitisTarihi.ToShortDateString()}, Ortalama İlerleme: %{OrtalamaIlerleme:F2}, " +
                   $"Görev Sayısı: {Gorevler.Count}";
        }
    }

    public class IsTakipSistemi
    {
        public List<Proje> Projeler { get; set; }
        public List<Calisan> Calisanlar { get; set; }
        private int nextProjeId = 1;
        private int nextGorevId = 1;
        private int nextCalisanId = 1;

        public IsTakipSistemi()
        {
            Projeler = new List<Proje>();
            Calisanlar = new List<Calisan>();
        }

        public void ProjeOlustur()
        {
            Console.Write("Proje Adı: ");
            string projeAdi = Console.ReadLine();

            Console.Write("Başlangıç Tarihi (gg/aa/yyyy): ");
            DateTime baslangic;
            while (!DateTime.TryParse(Console.ReadLine(), out baslangic))
            {
                Console.Write("Geçersiz tarih! Tekrar giriniz (gg/aa/yyyy): ");
            }

            Console.Write("Bitiş Tarihi (gg/aa/yyyy): ");
            DateTime bitis;
            while (!DateTime.TryParse(Console.ReadLine(), out bitis))
            {
                Console.Write("Geçersiz tarih! Tekrar giriniz (gg/aa/yyyy): ");
            }

            Proje yeniProje = new Proje(nextProjeId++, projeAdi, baslangic, bitis);
            Projeler.Add(yeniProje);
            Console.WriteLine("Proje başarıyla oluşturuldu:");
            Console.WriteLine(yeniProje);
        }

        public void CalisanEkle()
        {
            Console.Write("Çalışan Adı: ");
            string ad = Console.ReadLine();
            Console.Write("Çalışan Soyadı: ");
            string soyad = Console.ReadLine();

            Calisan yeniCalisan = new Calisan(nextCalisanId++, ad, soyad);
            Calisanlar.Add(yeniCalisan);
            Console.WriteLine("Çalışan başarıyla eklendi:");
            Console.WriteLine(yeniCalisan);
        }


        public void GorevOlustur()
        {
            Console.Write("Görev oluşturulacak Proje ID'sini giriniz: ");
            int projeId;
            if (!int.TryParse(Console.ReadLine(), out projeId))
            {
                Console.WriteLine("Geçersiz Proje ID!");
                return;
            }
            Proje proje = Projeler.Find(p => p.ProjeId == projeId);
            if (proje == null)
            {
                Console.WriteLine("Proje bulunamadı.");
                return;
            }
            Console.Write("Görev Adı: ");
            string gorevAdi = Console.ReadLine();
            Gorev yeniGorev = new Gorev(nextGorevId++, gorevAdi);
            proje.GorevEkle(yeniGorev);
            Console.WriteLine("Görev başarılı şekilde oluşturuldu ve projeye eklendi:");
            Console.WriteLine(yeniGorev);
        }


        public void GorevAtama()
        {
            Console.Write("Atanacak Görev ID'sini giriniz: ");
            int gorevId;
            if (!int.TryParse(Console.ReadLine(), out gorevId))
            {
                Console.WriteLine("Geçersiz Görev ID!");
                return;
            }
            Gorev hedefGorev = null;
            foreach (var proje in Projeler)
            {
                hedefGorev = proje.Gorevler.Find(g => g.GorevId == gorevId);
                if (hedefGorev != null)
                    break;
            }
            if (hedefGorev == null)
            {
                Console.WriteLine("Görev bulunamadı.");
                return;
            }
            Console.Write("Göreve atanacak Çalışan ID'sini giriniz: ");
            int calisanId;
            if (!int.TryParse(Console.ReadLine(), out calisanId))
            {
                Console.WriteLine("Geçersiz Çalışan ID!");
                return;
            }
            Calisan calisan = Calisanlar.Find(c => c.CalisanId == calisanId);
            if (calisan == null)
            {
                Console.WriteLine("Çalışan bulunamadı.");
                return;
            }
            hedefGorev.Sorumlu = calisan;
            calisan.Gorevler.Add(hedefGorev);
            Console.WriteLine("Görev başarılı şekilde çalışana atandı:");
            Console.WriteLine(hedefGorev);
        }


        public void IlerlemeGuncelle()
        {
            Console.Write("İlerleme güncellenecek Görev ID'sini giriniz: ");
            int gorevId;
            if (!int.TryParse(Console.ReadLine(), out gorevId))
            {
                Console.WriteLine("Geçersiz Görev ID!");
                return;
            }
            Gorev hedefGorev = null;
            foreach (var proje in Projeler)
            {
                hedefGorev = proje.Gorevler.Find(g => g.GorevId == gorevId);
                if (hedefGorev != null)
                    break;
            }
            if (hedefGorev == null)
            {
                Console.WriteLine("Görev bulunamadı.");
                return;
            }
            Console.Write("Yeni ilerleme değerini (0-100) giriniz: ");
            int ilerleme;
            if (!int.TryParse(Console.ReadLine(), out ilerleme))
            {
                Console.WriteLine("Geçersiz ilerleme değeri!");
                return;
            }
            hedefGorev.IlerlemeKaydet(ilerleme);
            Console.WriteLine("Görev güncellendi:");
            Console.WriteLine(hedefGorev);
        }


        public void ProjeleriListele()
        {
            if (Projeler.Count == 0)
            {
                Console.WriteLine("Sistemde kayıtlı proje bulunmamaktadır.");
                return;
            }
            foreach (var proje in Projeler)
            {
                Console.WriteLine(proje);
                Console.WriteLine("Görevler:");
                if (proje.Gorevler.Count == 0)
                {
                    Console.WriteLine("  - Görev bulunmamaktadır.");
                }
                else
                {
                    foreach (var gorev in proje.Gorevler)
                    {
                        Console.WriteLine("  " + gorev);
                    }
                }
                Console.WriteLine(new string('-', 40));
            }
        }


        public void CalisanGorevleriniListele()
        {
            Console.Write("Görevleri listelemek istediğiniz Çalışan ID'sini giriniz: ");
            int calisanId;
            if (!int.TryParse(Console.ReadLine(), out calisanId))
            {
                Console.WriteLine("Geçersiz Çalışan ID!");
                return;
            }
            Calisan calisan = Calisanlar.Find(c => c.CalisanId == calisanId);
            if (calisan == null)
            {
                Console.WriteLine("Çalışan bulunamadı.");
                return;
            }
            Console.WriteLine("Çalışanın görevleri:");
            if (calisan.Gorevler.Count == 0)
            {
                Console.WriteLine("  Atanmış görev bulunmamaktadır.");
            }
            else
            {
                foreach (var gorev in calisan.Gorevler)
                {
                    Console.WriteLine(gorev);
                }
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {

            PerformLogin();

            IsTakipSistemi sistem = new IsTakipSistemi();


            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- İş Takip ve Yönetim Sistemi ---");
                Console.WriteLine("1. Proje Oluştur");
                Console.WriteLine("2. Çalışan Ekle");
                Console.WriteLine("3. Görev Oluştur");
                Console.WriteLine("4. Görev Atama");
                Console.WriteLine("5. İlerleme Güncelle");
                Console.WriteLine("6. Projeleri Listele");
                Console.WriteLine("7. Çalışanların Görevlerini Listele");
                Console.WriteLine("8. Çıkış");
                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        sistem.ProjeOlustur();
                        break;
                    case "2":
                        sistem.CalisanEkle();
                        break;
                    case "3":
                        sistem.GorevOlustur();
                        break;
                    case "4":
                        sistem.GorevAtama();
                        break;
                    case "5":
                        sistem.IlerlemeGuncelle();
                        break;
                    case "6":
                        sistem.ProjeleriListele();
                        break;
                    case "7":
                        sistem.CalisanGorevleriniListele();
                        break;
                    case "8":
                        Console.WriteLine("Sistemden çıkılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyiniz.");
                        break;
                }
                Console.WriteLine("\nDevam etmek için bir tuşa basınız...");
                Console.ReadKey();
            }
        }

        static void PerformLogin()
        {
            int maxAttempts = 3;
            int attempt = 0;
            bool isAuthenticated = false;

            while (attempt < maxAttempts && !isAuthenticated)
            {
                Console.Clear();
                WriteHeader();

                Console.Write("Kullanıcı Adı: ");
                string username = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Kullanıcı adı boş olamaz.");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Parola: ");
                string password = ReadPassword();
                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Parola boş olamaz.");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                if (username.Equals("batu", StringComparison.OrdinalIgnoreCase) && password == "acar123")
                {
                    isAuthenticated = true;
                    Console.WriteLine("\nYönetici olarak giriş başarılı.");
                    Console.WriteLine("\nGiriş işlemi tamamlandı. Ana menüye yönlendiriliyorsunuz...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nGiriş bilgileri hatalı. Tekrar deneyiniz.");
                    attempt++;
                    if (attempt < maxAttempts)
                    {
                        Console.WriteLine($"Kalan deneme hakkınız: {maxAttempts - attempt}");
                    }
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                }
            }
            if (!isAuthenticated)
            {
                Console.WriteLine("Çok fazla hatalı giriş denemesi. Program sonlandırılıyor.");
                Environment.Exit(0);
            }
        }

        static void WriteHeader()
        {
            Console.WriteLine("***********************************************");
            Console.WriteLine("    İş Takip ve Yönetim Sistemi              ");
            Console.WriteLine("***********************************************\n");
        }

        static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password.Remove(password.Length - 1, 1);
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    Console.Write("*");
                    password.Append(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password.ToString();
        }
    }
}