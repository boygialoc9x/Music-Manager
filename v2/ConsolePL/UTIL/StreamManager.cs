using System;
using System.IO;
using Newtonsoft.Json;
using Persistence;
using System.Collections;

public class StreamManager
{
    public void WriteEncryptStringAtLayer2_Js(string encryptString, string fileName)
    {
        File.WriteAllText(fileName, JsonConvert.SerializeObject(encryptString));
    }
    public void WriteEncryptData_Js(Customer customer, string fileName)
    {
        File.WriteAllText(fileName, JsonConvert.SerializeObject(customer) );
    }
    public void WriteEncryptStringAtLayer2(string encryptString, string fileName)
    {
        File.WriteAllText(fileName, encryptString);
    }

    public string ReadEncryptStringAtLayer2(string fileName)
    {
        return File.ReadAllText(fileName);
    }
    public string ReadEncryptStringAtLayer2_Js(string fileName)
    {
        return JsonConvert.DeserializeObject<string>(File.ReadAllText(fileName));
    }
    public Customer ReadEncryptData_Js(string fileName)
    {
        return JsonConvert.DeserializeObject<Customer>(File.ReadAllText(fileName));
    }

    public bool Check(string fileName)
    {
        return File.Exists(fileName);
    }
    public void ClearAllData(string fileName)
    {
        File.WriteAllText(fileName, String.Empty);
    }
    // public ArrayList ReadHeperGmail(string fileName)
    // {
    //     ArrayList hpgm = new ArrayList();
    //     if (Check(fileName))
    //     {
    //         try
    //         {
    //             hpgm = JsonConvert.DeserializeObject<ArrayList>(File.ReadAllText(fileName));
    //         }
    //         catch
    //         {
    //             hpgm = null;
    //         }
    //     }
    //     else
    //     {
    //         hpgm = null;
    //     }
    //     return hpgm;
    // }
    // public void a21s()
    // {
    //     ArrayList a = new ArrayList();
    //     a.Add($"7z6mSGXEaQbxsx1ahUPyYTBosLUwtHFIKX5I"+@"\"+"hPI9J0=");
    //     a.Add("l7EJk8rP3+SA84lzJSzCqogyVzn0VNYq+GVqlGuc6RA=");
    //     File.WriteAllText("Helper Gmail.txt", JsonConvert.SerializeObject(a) );
    // }
}


