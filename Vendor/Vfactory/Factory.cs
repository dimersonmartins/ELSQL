using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vendor.Vfactory
{
    public class Factory
    {
        public List<Fake> DadosFalsos(int limit)
        {
            List<Fake> fakes = new List<Fake>();

            int count = 0;
            JObject jObject = new JObject(new JProperty("lista", JArray.Parse(MockJson.Get())));
            foreach (var item in jObject["lista"])
            {
                count++;

                if (count > limit)
                {
                    break;
                }

                string firstname = item["name"].ToString().ToLower().Replace(" ", "_");
                string secondname = item["capital"].ToString().ToLower().Replace(" ", "_");
                if(firstname.Length > 30)
                    firstname = firstname.Substring(0, 30);

                if (secondname.Length > 30)
                    secondname = secondname.Substring(0, 30);
                string email = firstname + "@" + secondname + ".com";

                Fake fake = new Fake();
                fake.nome = item["name"].ToString();
                fake.email = email;
                fake.regiao = item["region"].ToString();
                fake.cnpj = FormatCNPJ(long.Parse(GerarCNPJ()));
                fake.cpf = FormatCPF(long.Parse(GerarCpf()));
                fake.data = DateTime.Now;
                fake.cep = GerarCep();
                fake.telefone = GerarTelefone();
                fake.numeroInt = int.Parse(Numeric(8));
                fake.numeroDouble = double.Parse(Numeric(3) + "." + Numeric(2));
                fakes.Add(fake);
            }

            return fakes;
        }

        private string GerarTelefone()
        {
            return Numeric(4) + "-"+ Numeric(4);
        }
        private string GerarCep()
        {
            return Numeric(5) +"-"+Numeric(3);
        }

        private string Numeric(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string GerarCpf()
        {
            int soma = 0, resto = 0;
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            Random rnd = new Random();
            string semente = rnd.Next(100000000, 999999999).ToString();

            for (int i = 0; i < 9; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            semente = semente + resto;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            semente = semente + resto;
            return semente;
        }

        private string GerarCNPJ()
        {
            Random rnd = new Random();
            int n1 = rnd.Next(0, 10);
            int n2 = rnd.Next(0, 10);
            int n3 = rnd.Next(0, 10);
            int n4 = rnd.Next(0, 10);
            int n5 = rnd.Next(0, 10);
            int n6 = rnd.Next(0, 10);
            int n7 = rnd.Next(0, 10);
            int n8 = rnd.Next(0, 10);
            int n9 = 0;
            int n10 = 0;
            int n11 = 0;
            int n12 = 1;

            int Soma1 = n1 * 5 + n2 * 4 + n3 * 3 + n4 * 2 + n5 * 9 + n6 * 8 + n7 * 7 + n8 * 6 + n9 * 5 + n10 * 4 + n11 * 3 + n12 * 2;

            int DV1 = Soma1 % 11;

            if (DV1 < 2)
            {
                DV1 = 0;
            }
            else
            {
                DV1 = 11 - DV1;
            }

            int Soma2 = n1 * 6 + n2 * 5 + n3 * 4 + n4 * 3 + n5 * 2 + n6 * 9 + n7 * 8 + n8 * 7 + n9 * 6 + n10 * 5 + n11 * 4 + n12 * 3 + DV1 * 2;

            int DV2 = Soma2 % 11;

            if (DV2 < 2)
            {
                DV2 = 0;
            }
            else
            {
                DV2 = 11 - DV2;
            }

            return n1.ToString() + n2 + n3 + n4 + n5 + n6 + n7 + n8 + n9 + n10 + n11 + n12 + DV1 + DV2;
        }


        public static string FormatCNPJ(long CNPJ)
        {
            return CNPJ.ToString(@"00\.000\.000\/0000\-00");
        }

        public static string FormatCPF(long CPF)
        {
            return CPF.ToString(@"000\.000\.000\-00");
        }
    }

    public class Fake
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string regiao { get; set; }
        public string telefone { get; set; }
        public DateTime data { get; set; }
        public string cpf { get; set; }
        public string cnpj { get; set; }
        public string cep { get; set; }
        public int numeroInt { get; set; }
        public double numeroDouble { get; set; }
    }
}
