using System;
using System.Collections.Generic;

// Interface untuk kemampuan
public interface IKemampuan
{
    string Nama { get; }
    int Cooldown { get; }
    void Gunakan(Robot target);
}

// Abstract class Robot
public abstract class Robot
{
    public string Nama { get; protected set; }
    public int Energi { get; protected set; }
    public int Armor { get; protected set; }
    public int Serangan { get; protected set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        this.Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public void SetEnergi(int nilai)
    {
        Energi = nilai;
    }

    public void SetArmor(int nilai)
    {
        Armor = nilai;
    }

    public void Serang(Robot target)
    {
        int damage = Serangan - target.Armor;
        if (damage < 0) damage = 0;
        target.SetEnergi(target.Energi - damage);  
        Console.WriteLine($"{Nama} menyerang {target.Nama} dan memberikan {damage} damage.");
    }

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }

    public abstract void GunakanKemampuan(IKemampuan kemampuan, Robot target);
}

public class BosRobot : Robot
{
    public int Pertahanan { get; private set; }

    public BosRobot(string nama, int energi, int pertahanan, int serangan)
        : base(nama, energi, pertahanan, serangan)
    {
        Pertahanan = pertahanan;
    }

    public void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan - Pertahanan;
        if (damage < 0) damage = 0;
        SetEnergi(Energi - damage);  
        Console.WriteLine($"{Nama} menerima {damage} damage dari {penyerang.Nama}.");
        if (Energi <= 0) Mati();
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah dikalahkan!");
        Console.WriteLine("Permainan berakhir, Boss Techno telah dikalahkan O__O!");
    }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        kemampuan.Gunakan(target);
    }

    public void SerangRobot(Robot target)
    {
        Serang(target);
    }
}

// Beberapa kemampuan
public class Perbaikan : IKemampuan
{
    public string Nama => "Perbaikan";
    public int Cooldown { get; private set; } = 2;

    public void Gunakan(Robot target)
    {
        target.SetEnergi(target.Energi + 20);  
        Console.WriteLine($"{target.Nama} menggunakan {Nama} dan memulihkan 20 energi.");
    }
}

public class SeranganListrik : IKemampuan
{
    public string Nama => "Serangan Listrik";
    public int Cooldown { get; private set; } = 2;

    public void Gunakan(Robot target)
    {
        int damage = 15 - target.Armor;
        if (damage < 0) damage = 0;
        target.SetEnergi(target.Energi - damage);  
        Console.WriteLine($"{target.Nama} terkena {Nama} dan menerima {damage} damage.");
    }
}

public class PlasmaCannon : IKemampuan
{
    public string Nama => "Plasma Cannon";
    public int Cooldown { get; private set; } = 2;

    public void Gunakan(Robot target)
    {
        int damage = 25;
        target.SetEnergi(target.Energi - damage);  
        Console.WriteLine($"{target.Nama} terkena {Nama} dan menerima {damage} damage.");
    }
}

public class PertahananSuper : IKemampuan
{
    public string Nama => "Pertahanan Super";
    public int Cooldown { get; private set; } = 4;

    public void Gunakan(Robot target)
    {
        target.SetArmor(target.Armor + 10);  
        Console.WriteLine($"{target.Nama} menggunakan {Nama} dan meningkatkan armor sebesar 10.");
    }
}

public class RobotBiasa : Robot
{
    public RobotBiasa(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan)
    {
    }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        kemampuan.Gunakan(target);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Robot robot1 = new RobotBiasa("Robot Rerez", 100, 10, 20);
        Robot robot2 = new RobotBiasa("Robot Beta", 100, 8, 22);
        BosRobot bos = new BosRobot("Bos Techno", 200, 20, 30);

        List<IKemampuan> kemampuanList = new List<IKemampuan>
        {
            new Perbaikan(),
            new SeranganListrik(),
            new PlasmaCannon(),
            new PertahananSuper()
        };

        // Loop permainan sederhana
        bool gameRunning = true;
        while (gameRunning)
        {
            Console.WriteLine("SELAMAT DATANG DI ROBO RANGER!\n");
            Console.WriteLine("================== KLASIFIKASI ROBOT ================== ");
            robot1.CetakInformasi();
            robot2.CetakInformasi();
            bos.CetakInformasi();
            Console.WriteLine("======================================================= ");

            Console.WriteLine("\n------------ AYO BERTARUNG! ------------");
            Console.WriteLine("\n1. Robot Rerez menyerang Bos Techno");
            Console.WriteLine("2. Robot Beta menyerang Bos Techno");
            Console.WriteLine("3. Bos Techno menyerang Robot Rerez");
            Console.WriteLine("4. Bos Techno menyerang Robot Beta");
            Console.WriteLine("5. Robot Rerez menggunakan kemampuan");
            Console.WriteLine("6. Robot Beta menggunakan kemampuan");
            Console.WriteLine("7. Bos Techno menggunakan kemampuan");
            Console.WriteLine("8. Keluar");

            Console.Write("Pilih: ");
            int pilihan = int.Parse(Console.ReadLine());

            switch (pilihan)
            {
                case 1:
                    robot1.Serang(bos);
                    break;
                case 2:
                    robot2.Serang(bos);
                    break;
                case 3:
                    bos.SerangRobot(robot1);
                    break;
                case 4:
                    bos.SerangRobot(robot2);
                    break;
                case 5:
                    Console.WriteLine("\nPilih kemampuan: ");
                    Console.WriteLine("1. Perbaikan");
                    Console.WriteLine("2. Serangan Listrik");
                    Console.WriteLine("3. Plasma Cannon");
                    Console.WriteLine("4. Pertahanan");
                    int k1 = int.Parse(Console.ReadLine()) - 1;

                    if (k1 == 1 || k1 == 2 || k1 == 3) 
                    {
                        robot1.GunakanKemampuan(kemampuanList[k1], bos); 
                    }
                    else 
                    {
                        robot1.GunakanKemampuan(kemampuanList[k1], robot1); 
                    }
                    break;
                case 6:
                    Console.WriteLine("\nPilih kemampuan: ");
                    Console.WriteLine("1. Perbaikan");
                    Console.WriteLine("2. Serangan Listrik");
                    Console.WriteLine("3. Plasma Cannon");
                    Console.WriteLine("4. Pertahanan");
                    int k2 = int.Parse(Console.ReadLine()) - 1;

                    if (k2 == 1 || k2 == 2 || k2 == 3) 
                    {
                        robot2.GunakanKemampuan(kemampuanList[k2], bos); 
                    }
                    else 
                    {
                        robot2.GunakanKemampuan(kemampuanList[k2], robot2); 
                    }
                    break;
                case 7:
                    Console.WriteLine("\nPilih kemampuan: ");
                    Console.WriteLine("1. Perbaikan");
                    Console.WriteLine("2. Serangan Listrik");
                    Console.WriteLine("3. Plasma Cannon");
                    Console.WriteLine("4. Pertahanan");
                    int k3 = int.Parse(Console.ReadLine()) - 1;

                    if (k3 == 1 || k3 == 2 || k3 == 3) 
                    {
                        bos.GunakanKemampuan(kemampuanList[k3], robot1); 
                    }
                    else 
                    {
                        bos.GunakanKemampuan(kemampuanList[k3], bos); 
                    }
                    break;
                case 8:
                    gameRunning = false;
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                    break;
            }

            if (robot1.Energi <= 0 || robot2.Energi <= 0 || bos.Energi <= 0)
            {
                gameRunning = false;
            }

            Console.WriteLine();
        }

        Console.WriteLine("Terima kasih telah bermain!");
    }
}

