using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using VSAX3; 

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
[ProgId("ComtecXmlValidate.Class1")]

public class Class1
{ 
    public string CheckXmlFile(string xmlFile, string xsdFile)
    {
        //FileStream LogFile = new FileStream("ComtecXml.ini", FileMode.Create, FileAccess.Write);
        //StreamWriter sw = new StreamWriter(LogFile);
        var pSw = new Stopwatch();
        pSw.Start();
        try
        {
            var pXml = new FileStream(xmlFile, FileMode.Open, FileAccess.Read);
            var pXsd = new System.Xml.Schema.XmlSchemaSet();
            pXsd.Add("", xsdFile);
            {
                //SnpXmlValidatingReader:
                var reader = new SnpXmlValidatingReader { ValidatingType = EVsaxValidateType.SaxSchematronUsch };
                string StateValidate;
                StateValidate = reader.Validate(pXml, Path.GetFileName(xmlFile), pXsd)
                    ? "1"
                    : "0";
                if (StateValidate == "0")
                {
                    pXml.Close();
                    return "-1";
                }
                if (StateValidate == "1")
                {
                    var errors = reader.ErrorHandler;
                    if (errors.Errors.Count == 0)
                    {
                        pXml.Close();
                        return "1";
                    }
                    else
                    {
                        string ErrorsToComtec = "Всего ошибок: " + errors.Errors.Count.ToString() + "\r\n";
                        int i = 0000;
                        foreach (var e in errors.Errors)
                        {
                            i++;
                            ErrorsToComtec = ErrorsToComtec + i.ToString() + ". " + e.ErrorText.Replace("зрения", "точки зрения") + "\r\n";       
                        }
                        reader.Close();
                        pXml.Close();
                        return ErrorsToComtec;
                    }
                }
            }
            
        }
        catch (System.Xml.XmlException e)
        {
            return "-1";
        }
        catch (System.Xml.Schema.XmlSchemaException e)
        {
            return "-1";
        }
        catch (FileNotFoundException ioEx)
        {
            return "-1";
        }
        catch (Exception e)
        {
            return "-1";
        }
        return "-1";
    }
}

