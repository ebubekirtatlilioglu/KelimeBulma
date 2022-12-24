using System.Data.SqlClient;

SqlConnection con;
SqlCommand cmd;
con=new SqlConnection("server=.; database=SozluklerDb; Integrated Security=true");
con.Open();
Console.WriteLine("============================Kelime Bulma Uygulamamıza Hoşgeldiniz============================");
Console.WriteLine();
Console.WriteLine("Bu Uygulama Sayesinde girdiğiniz harflerden oluşan Türkçe Kelimeleri Hızlıca Bulacaksınız.");
Console.WriteLine();
Console.WriteLine("Başlamak İçin Bir Tuşa Basmanız Yeterli");
Console.ReadKey();
Console.WriteLine();
while (true)
{
    Console.WriteLine("Aramak İstediğiniz Harfleri Giriniz");
    string harfler = Console.ReadLine().ToLower();
    Dictionary<char, int> sonuc = new Dictionary<char, int>();
    foreach (char harf in harfler)
    {
        if (sonuc.ContainsKey(harf))
        {
            sonuc[harf]++;
        }
        else
        {
            sonuc.Add(harf, 1);
        }
    }
    string cmdText = "Select  Sozcuk from TurkceSozluk where Sozcuk not like @p1 and len(Sozcuk)<=@p2";
    foreach (var cift in sonuc)
    {
        cmdText += $" and len(Sozcuk)-len(replace(Sozcuk,'{cift.Key}',''))<={cift.Value}";
    }
    cmdText += "order by len(Sozcuk),Sozcuk";
    cmd = new SqlCommand(cmdText, con);
    cmd.Parameters.AddWithValue("@p1", $"%[^{harfler}]%");
    cmd.Parameters.AddWithValue("@p2", harfler.Length);
    var dr = cmd.ExecuteReader();
    while (dr.Read())
        Console.WriteLine(dr[0]);
    dr.Close();
    Console.WriteLine("Yeni Bir Arama Yapmak İster Misiniz?(Evet/Hayır)");
    string cevap=Console.ReadLine().ToLower();
    if (cevap=="evet")
    {
        continue;
    }
    else
    {
        return;
    }
}



