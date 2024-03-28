using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Renomear
{
    class Program
    {
        public static string Raiz { get; set; } = "";
        public static int max_arq { get; set; } = 50;
        public static int max_pasta { get; set; } = 50;

        public static string Perguntar(string pergunta)
        {
            Console.WriteLine(pergunta);

            // Create a string variable and get user input from the keyboard and store it in the variable
            string resposta = Console.ReadLine();
            return resposta.ToUpper();
        }

        static void Main(string[] args)
        {
        inicio:
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("Renomeia Recursivo v1.0");
            Console.WriteLine("Copie os arquivos para uma outra pasta.");
            Console.WriteLine("O Programa substitui os arquivos.");

            Console.WriteLine("===================================!");
            Raiz = Perguntar("Defina o Diretório Raiz:");

            if (Directory.Exists(Raiz))
            {

                if (Raiz.Contains(@"\") && !Raiz.EndsWith(@"\"))
                {
                    Raiz = Raiz + @"\";
                }

                if (Raiz.Contains(@"/") && !Raiz.EndsWith(@"/"))
                {
                    Raiz = Raiz + @"/";
                }

                Remover_Caracteres_Especiais = Perguntar("Remover Caracteres Especiais de arquivos? (S ou N)");
                Pastas_Renomear = Perguntar("Restringir comprimento do nome das Pastas? (S ou N)");
                Arquivos_Renomear = Perguntar("Restringir comprimento do nome dos Arquivos? (S ou N)");
                Apagar_Pastas_Vazias = Perguntar("Apagar pastas vazias? (S ou N)");


                TudoMaiusculo = "S";
                SubstituirChar = Perguntar("Digite os caracteres que deseja substituir. (ENTER) para nenhum").ToUpper();
                if (SubstituirChar.Length>0)
                {
                    SubstituirCharPor = Perguntar($"Digite os caracteres que deseja colocar no lugar de [{SubstituirChar}]").ToUpper();
                }

                Excluir_Arquivos_Temporarios = Perguntar("Excluir arquivos temporarios? (S ou N)");

                if (Arquivos_Renomear.StartsWith("S"))
                {
                    max_arq = GetInt(Perguntar("Digite o comprimento máximo de caracteres para os arquivos:"));

                    if (max_arq < 3)
                    {
                        Console.WriteLine("Valor inválido.");
                        Arquivos_Renomear = "N";
                    }
                }

                if (Pastas_Renomear.StartsWith("S"))
                {
                    max_pasta = GetInt(Perguntar("Digite o comprimento máximo de caracteres para as pastas:"));

                    if (max_pasta < 8)
                    {
                        Console.WriteLine("Valor inválido.");
                        Pastas_Renomear = "N";
                    }
                    else
                    {
                        //Remover_Caracteres_Especiais_Pasta = Perguntar("Remover Caracteres Especiais de pastas? (S ou N)");
                    }
                }


                VamoTrabaia().Wait();
                Console.Read();
                goto inicio;
            }
            else
            {
                Console.WriteLine("Diretório Inválido");
                goto inicio;
            }

        }
        public static double GetDouble(object comp, int Decimais = 4)
        {
            if (comp == null)
            {
                return 0;
            }

            try
            {

                double val;
                if (double.TryParse(comp.ToString().Replace(" ", "").Replace("%", "").Replace("@", "").Replace("#", ""), System.Globalization.NumberStyles.Float, BR, out val))
                {
                    try
                    {
                        return Math.Round(val, Decimais);

                    }
                    catch (Exception)
                    {

                        return val;
                    }
                }

                else if (double.TryParse(comp.ToString().Replace(" ", "").Replace("%", "").Replace("@", "").Replace("#", ""), System.Globalization.NumberStyles.Float, US, out val))
                {
                    try
                    {

                        return Math.Round(val, Decimais);
                    }
                    catch (Exception)
                    {
                        return val;
                    }
                }
                else return 0;
            }
            catch (Exception)
            {

                return 0;
            }


        }
        private static System.Globalization.CultureInfo US = new System.Globalization.CultureInfo("en-US");
        private static System.Globalization.CultureInfo BR = new System.Globalization.CultureInfo("pt-BR");
        public static int GetInt(object comp)
        {

            if (comp == null) { return 0; }
            string comps = comp.ToString().Replace(" ", "");
            if (comps == "") { comps = "0"; }
            try
            {
                return Convert.ToInt32(Math.Ceiling(GetDouble(comps.Replace(".", ","))));
            }
            catch (Exception ex)
            {

                return 0;
            }

        }
        public static string Pastas_Renomear { get; set; } = "N";
        public static string Arquivos_Renomear { get; set; } = "N";
        public static string Apagar_Pastas_Vazias { get; set; } = "N";
        public static string Remover_Caracteres_Especiais { get; set; } = "N";
        public static string Remover_Caracteres_Especiais_Pasta { get; set; } = "N";
        public static string Excluir_Arquivos_Temporarios { get; set; } = "N";
        public static string TudoMaiusculo { get; set; } = "N";
        public static string SubstituirChar { get; set; } = "";
        public static string SubstituirCharPor { get; set; } = "";


        public static bool Pergunta(string Pergunta, string Titulo = "Confirme")
        {
            return System.Windows.MessageBox.Show(
                             Pergunta,
                             Titulo,
                             MessageBoxButton.YesNo,
                             MessageBoxImage.Question,
                             MessageBoxResult.Yes, System.Windows.MessageBoxOptions.ServiceNotification) == MessageBoxResult.Yes;
        }
        public static string SelecionarPasta(string titulo = "Selecione", string pastaraiz = "")
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = titulo;
            dialog.SelectedPath = pastaraiz;
            dialog.ShowNewFolderButton = true;
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (Directory.Exists(dialog.SelectedPath))
                {
                    return dialog.SelectedPath;
                }
            }
            return "";
        }
        public static async Task VamoTrabaia()
        {

            await Task.Run(() =>
            {
                if (Directory.Exists(Raiz))
                {
                    AjustarPasta(Raiz);
                    Console.WriteLine("==============================");
                    Console.WriteLine("Finalizado!");
                    Console.WriteLine("===================================!");
                }
                else
                {
                    Console.WriteLine($"PASTA INVÁLIDA ==> {Raiz} ");
                }

            });
        }

        private static void AjustarPasta(string pasta)
        {
            string npasta = pasta;
            if (Pastas_Renomear.StartsWith("S"))
            {
                npasta = Renomear(pasta, true);
            }


            var arquivos = Directory.GetFiles(npasta, "*").ToList();
            if (Arquivos_Renomear.StartsWith("S"))
            {

                foreach (var arq in arquivos)
                {
                    Renomear(arq, false);
                }
            }

            if (Remover_Caracteres_Especiais.StartsWith("S") | (SubstituirChar.Length>0))
            {
                arquivos = Directory.GetFiles(npasta, "*").ToList();
                foreach (var arquivo in arquivos)
                {
                    var extensao = getExtensao(arquivo);
                    var nome = getNome(arquivo);
                    int cont = 0;
                    var novo_nome = arquivo;
                    if (SubstituirChar.Length > 0)
                    {
                        novo_nome = novo_nome.Replace(SubstituirChar, SubstituirCharPor);
                    }
                    if (Remover_Caracteres_Especiais.StartsWith("S"))
                    {
                        novo_nome = npasta + RemoverCaracteresEspeciais(nome) + extensao;
                    }


                    if (novo_nome.ToUpper() != arquivo.ToUpper())
                    {
                        while (File.Exists(novo_nome))
                        {
                            cont++;
                            novo_nome = npasta + RemoverCaracteresEspeciais(nome) + "_" + cont + extensao;
                        }

                        if (TudoMaiusculo.StartsWith("S"))
                        {
                            novo_nome = novo_nome.ToUpper();
                        }

     
                        try
                        {
                            Directory.Move(arquivo, novo_nome);
                            Console.WriteLine($"RENOMEADO ==> {arquivo} renomeado para {novo_nome}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"ERRO ==> {arquivo} => {ex.Message}");

                        }

                    }

                }
            }
            List<string> sub_pastas = GetSubPastas(npasta);

            foreach (var s in sub_pastas)
            {
                AjustarPasta(s);
            }


            if (Apagar_Pastas_Vazias.StartsWith("S"))
            {
                if (arquivos.Count == 0 && sub_pastas.Count == 0)
                {
                    try
                    {
                        Directory.Delete(npasta);
                        Console.WriteLine($"PASTA VAZIA APAGADO ==> {npasta}");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }



        }

        private static List<string> GetSubPastas(string npasta)
        {
            var s = Directory.GetDirectories(npasta, "*").ToList();
            List<string> retorno = new List<string>();
            foreach (var p in s)
            {
                string pps = p;
                if (pps.Contains(@"\") && !pps.EndsWith(@"\"))
                {
                    pps = pps + @"\";
                }
                retorno.Add(pps);

                if (pps.Contains(@"/") && !pps.EndsWith(@"/"))
                {
                    pps = pps + @"/";
                }
                retorno.Add(pps);
            }
            return retorno;
        }



        public static string RemoverCaracteresEspeciais(string valor)
        {
            string retorno = valor;

            retorno = retorno.TrimEnd();
            retorno = retorno.TrimStart();

            List<List<string>> de_para = new List<List<string>>();
            de_para.Add(new List<string> { "á", "a" });
            de_para.Add(new List<string> { "é", "e" });
            de_para.Add(new List<string> { "í", "i" });
            de_para.Add(new List<string> { "ó", "o" });
            de_para.Add(new List<string> { "ú", "u" });

            de_para.Add(new List<string> { "ã", "a" });
            de_para.Add(new List<string> { "õ", "o" });

            de_para.Add(new List<string> { "â", "a" });
            de_para.Add(new List<string> { "ê", "e" });
            de_para.Add(new List<string> { "î", "i" });
            de_para.Add(new List<string> { "ô", "o" });
            de_para.Add(new List<string> { "û", "u" });

            de_para.Add(new List<string> { "ä", "a" });
            de_para.Add(new List<string> { "ë", "e" });
            de_para.Add(new List<string> { "ï", "i" });
            de_para.Add(new List<string> { "ö", "o" });
            de_para.Add(new List<string> { "ü", "u" });

            de_para.Add(new List<string> { "Á", "A" });
            de_para.Add(new List<string> { "É", "E" });
            de_para.Add(new List<string> { "Í", "I" });
            de_para.Add(new List<string> { "Ó", "O" });
            de_para.Add(new List<string> { "Ú", "U" });
            de_para.Add(new List<string> { "Ç", "C" });

            de_para.Add(new List<string> { "Ã", "A" });
            de_para.Add(new List<string> { "Õ", "O" });

            de_para.Add(new List<string> { "Â", "A" });
            de_para.Add(new List<string> { "Ê", "E" });
            de_para.Add(new List<string> { "Î", "I" });
            de_para.Add(new List<string> { "Ô", "O" });
            de_para.Add(new List<string> { "Û", "U" });


            de_para.Add(new List<string> { "Ä", "A" });
            de_para.Add(new List<string> { "Ë", "E" });
            de_para.Add(new List<string> { "Ï", "I" });
            de_para.Add(new List<string> { "Ö", "O" });
            de_para.Add(new List<string> { "Ü", "U" });

            de_para.Add(new List<string> { "@", "_" });


            de_para.Add(new List<string> { "_", " " });
            de_para.Add(new List<string> { ".", " " });
            de_para.Add(new List<string> { "!", "" });
            de_para.Add(new List<string> { "#", "" });
            de_para.Add(new List<string> { "(", " " });
            de_para.Add(new List<string> { ")", " " });
            de_para.Add(new List<string> { "#", " " });
            de_para.Add(new List<string> { "º", "" });
            de_para.Add(new List<string> { "°", "" });
            de_para.Add(new List<string> { "=", " " });
            de_para.Add(new List<string> { "~", " " });
            de_para.Add(new List<string> { "+", " " });
            de_para.Add(new List<string> { "*", " " });
            de_para.Add(new List<string> { "_-_", " - " });
            de_para.Add(new List<string> { "  ", " " });
            de_para.Add(new List<string> { "  ", " " });
            de_para.Add(new List<string> { "  ", " " });
            de_para.Add(new List<string> { "  ", " " });
            de_para.Add(new List<string> { "  ", " " });




            foreach (var d in de_para)
            {
                retorno = retorno.Replace(d[0], d[1]);
            }

            retorno = retorno.TrimStart();
            retorno = retorno.TrimEnd();
            retorno = retorno.TrimStart('_');
            retorno = retorno.TrimEnd('_');
            retorno = retorno.TrimStart('-');
            retorno = retorno.TrimEnd('-');

            return retorno;

        }

        public static string getNome(string Arquivo)
        {
            try
            {

                Arquivo = Arquivo.TrimEnd(@"/".ToCharArray());
                Arquivo = Arquivo.TrimEnd(@"\".ToCharArray());
                var s = System.IO.Path.GetFileNameWithoutExtension(Arquivo);
                if (s == "")
                {
                    return Path.GetDirectoryName(Arquivo);
                }

                return s;
            }
            catch (Exception)
            {

            }
            return Arquivo;
        }
        public static string getUpdir(string Dir)
        {
            if (!Dir.EndsWith(@"\")) { Dir = Dir + @"\"; }
            var Raiz = System.IO.Path.GetFullPath(System.IO.Path.Combine(Dir, @"..\"));
            if (Raiz.Contains(@"\") && !Raiz.EndsWith(@"\"))
            {
                Raiz = Raiz + @"\";
            }

            if (Raiz.Contains(@"/") && !Raiz.EndsWith(@"/"))
            {
                Raiz = Raiz + @"/";
            }
            return Raiz;
        }
        public static string getPasta(string Arquivo)
        {
            try
            {
                var retorno = System.IO.Path.GetDirectoryName(Arquivo);

                if (!retorno.EndsWith(@"\") && retorno.Contains(@"\"))
                {
                    retorno = retorno + @"\";
                }
                else if (!retorno.EndsWith(@"/") && retorno.Contains(@"/"))
                {
                    retorno = retorno + @"/";
                }
                return retorno;
            }
            catch (Exception)
            {


            }
            return "";
        }
        public static string getExtensao(string Arquivo)
        {
            return System.IO.Path.GetExtension(Arquivo);
        }
        public static string CortarString(string Origem, int MaxComp, bool pontilhado = true)
        {
            string txt = Origem;
            if (Origem.Count() > MaxComp)
            {
                txt = Origem.Substring(0, MaxComp) + (pontilhado ? "..." : "");
            }
            return txt;
        }
        public static string Renomear(string nome_arquivo_ou_pasta, bool ispasta)
        {
            int max_arq = Program.max_arq;

            if (ispasta)
            {
                max_arq = Program.max_pasta;
            }

            string novo_nome = nome_arquivo_ou_pasta;


            if (nome_arquivo_ou_pasta.ToUpper().Replace(@"\", "").Replace(@"/", "").EndsWith(".GRP"))
            {
                return novo_nome;
            }
            var nome = getNome(nome_arquivo_ou_pasta);
            if (nome.Length > max_arq | nome_arquivo_ou_pasta.Length > 200)
            {
                var nome_pai = getUpdir(nome_arquivo_ou_pasta);

                List<string> excluir = new List<string> { "TEST_MARK.XML",
                                                          ".XTESTPOS",
                                                          ".XMARK",
                                                          ".STP",
                                                          ".$AC",
                                                          ".XTESTMARK",
                                                          ".DWT",
                                                          ".BAK",
                                                          ".LNK",
                                                          "THUMBS.DB",
                                                          "__BASE__.DWG",
                                                          ".LOG",
                                                          ".ERR",
                                                          ".DWL"
                                                            };
                string extensao = "";
                if (!ispasta)
                {
                    nome_pai = getPasta(nome_arquivo_ou_pasta);
                    extensao = getExtensao(nome_arquivo_ou_pasta);
                    var nome_completo = (nome + extensao).ToUpper();
                    if (extensao.ToUpper() == ".EXE"
                        | extensao.ToUpper() == ".DLL"
                        | extensao.ToUpper() == ".INI"
                        | extensao.ToUpper() == ".DB"
                        | extensao.ToUpper() == ".BAT"
                        | extensao.ToUpper() == ".RAR"
                        | extensao.ToUpper() == ".ZIP"

                        )
                    {
                        return nome_arquivo_ou_pasta;
                    }
                    if (Excluir_Arquivos_Temporarios.StartsWith("S"))
                    {
                        foreach (string e in excluir)
                        {
                            if (nome_completo.EndsWith(e))
                            {
                                try
                                {
                                    File.Delete(nome_arquivo_ou_pasta);
                                    Console.WriteLine($"EXCLUIDO====> {nome_arquivo_ou_pasta}");
                                    return nome_arquivo_ou_pasta;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"ERRO====> {ex.Message}\n{ex.StackTrace}");

                                }
                            }
                        }
                    }

                }

                var mmax = max_arq;




                if (nome_arquivo_ou_pasta.Length > 200)
                {
                    mmax = 200 - nome_pai.Length;
                }

                if (nome.Length >= mmax && mmax > 2)
                {
                    try
                    {
                        int cont = 0;

                        novo_nome = nome_pai + CortarString(nome, mmax, false) + "~" + cont + extensao;
                        if (novo_nome.ToUpper() != nome_arquivo_ou_pasta.ToUpper())
                        {
                            while (File.Exists(novo_nome))
                            {
                                cont++;
                                novo_nome = nome_pai + CortarString(nome, mmax, false) + "~" + cont + extensao;
                            }
                            Directory.Move(nome_arquivo_ou_pasta, novo_nome);
                            Console.WriteLine($"RENOMEADO ==> {nome_arquivo_ou_pasta} renomeado para {novo_nome}");
                        }


                        return novo_nome;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERRO ==> {nome_arquivo_ou_pasta} \n{ex.Message}\n{ex.StackTrace}");
                    }
                }
                else
                {
                    //Console.WriteLine($"MANTIDO ==> {nome_arquivo_ou_pasta}");

                }
            }



            return nome_arquivo_ou_pasta;
        }
    }




}
