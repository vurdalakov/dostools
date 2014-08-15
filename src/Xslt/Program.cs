namespace Vurdalakov
{
    using System;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;

    class Program
    {
        static void Main(String[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("XSLT processor | https://github.com/vurdalakov/dostools");
                Console.WriteLine("Usage:\n    XSLT <data file> <style sheet file> <output file> [/option1] [/option2] ...");
                Console.WriteLine("Example:\n    XSLT userdata.xml template.xsl userdata.html /EnableScript");
                Console.WriteLine("Options:\n/EnableScript - enables support for embedded script blocks in XSLT style sheets");
                return;
            }

            try
            {
                // set default settings

                XsltSettings xsltSettings = new XsltSettings();

                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    NewLineChars = Environment.NewLine,
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                // set custom settings

                for (int i = 3; i < args.Length; i++)
                {
                    switch (args[i].Substring(1).ToLower())
                    {
                        case "enablescript":
                            xsltSettings.EnableScript = true;
                            break;
                    }
                }

                // load XML document

                XPathDocument xPathDocument = new XPathDocument(args[0]);

                // load XML document

                XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
                xslCompiledTransform.Load(args[1], xsltSettings, null);

                // create output file writer

                XmlWriter xmlWriter = XmlWriter.Create(args[2], xmlWriterSettings);

                // perform transform

                xslCompiledTransform.Transform(xPathDocument, null, xmlWriter);
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Console.WriteLine("{0}: {1} ", ex.GetType().Name, ex.Message);
                    ex = ex.InnerException;
                }
            }
        }
    }
}
