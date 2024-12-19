using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FileManipulation
{
    class Program
    {
        static void Main()
        {
            string originDirectoryPath 
                   = @"C:\Users\PC\Documents\TechClass\FileManipulation\Origin";
            string outDirectoryPath 
                   = @"C:\Users\PC\Documents\TechClass\FileManipulation\Out";

            string originFilePath 
                   = @"C:\Users\PC\Documents\TechClass\FileManipulation\Origin\origin.csv";
            string outFilePath 
                   = @"C:\Users\PC\Documents\TechClass\FileManipulation\Out\summary.csv";

            string originFileContent = "TV LED,1290.99,1\n"+
                                       "Video Game Chair,350.50,3\n"+
                                       "Iphone X,900.00,2\n"+
                                       "Samsung Galaxy 9,850.00,2";

            FileManipulation(originFilePath, outFilePath, 
                             originDirectoryPath, outDirectoryPath, 
                             originFileContent);
        }

        static void FileManipulation(string originFilePath, string outFilePath, 
                                     string originDirectoryPath, string outDirectoryPath,
                                     string originFileContent)
        {
            if (!Directory.Exists(originDirectoryPath))
            {
                Directory.CreateDirectory(originDirectoryPath);
            }

            if (!Directory.Exists(outDirectoryPath))
            {
                Directory.CreateDirectory(outDirectoryPath);
            }

            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }

            string originFileText = null;
            List<Produto> produtos = new List<Produto>();
            if (!File.Exists(originFilePath))
            {
                using (FileStream fsOrigin = new FileStream(originFilePath, 
                                                            FileMode.OpenOrCreate, 
                                                            FileAccess.ReadWrite,
                                                            FileShare.Read))
                {
                    using (StreamWriter sw = new StreamWriter(fsOrigin, leaveOpen: true))
                    {
                        sw.Write(originFileContent);
                        sw.Flush();
                    }

                    fsOrigin.Position = 0;

                    using (StreamReader sr = new StreamReader(fsOrigin))
                    {
                        originFileText = sr.ReadToEnd();
                    }
                };

            }
            var itens = originFileText.Split('\n');

            foreach (var item in itens)
            {
                var itemSegmentado = item.Split(',');

                string nome = itemSegmentado[0];
                double preco = double.Parse(itemSegmentado[1], CultureInfo.InvariantCulture);
                int qnt = int.Parse(itemSegmentado[2]);
                
                Produto produto = new Produto()
                {
                    Nome = nome,
                    Preco = preco,
                    Quantidade = qnt
                };

                produtos.Add(produto);
            }
                using (FileStream fsOut = new FileStream(outFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter srOut = new StreamWriter(fsOut))
                    {
                        CultureInfo ci = new CultureInfo("pt-BR");
                        foreach (var produto in produtos)
                        {
                            srOut.WriteLine($"{produto.Nome},{(produto.Preco * produto.Quantidade).ToString("N2", ci)}");
                        }
                    }
                }
        }
    }
}