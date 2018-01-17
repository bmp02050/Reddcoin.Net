using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

/*
 * TODO: Add ELSE statement for param!=null
 * Switch (cmd)
 * If one of the commands that can run null, then add those
 * else
 * missing data
*/

namespace ReddCoinNet
{    
    public static class Errors
    {
        public static string errors { get; set; }
    }

    public static class Load
    {
        public static string u_name;
        public static string p_word;
        public static string ip { get; set; }
        public static string port { get; set; }
        public static string ConfigLocation { get; set; }
        public static string[] disregard { get; set; }
        public static string SQLConnection { get; set; }
        public static string path { get; set; }
        public static string wpa { get; set; }

        public static bool FileData()
        {
            bool result = false;
            Errors.errors = string.Empty;
            using (StreamReader sr = new StreamReader(ConfigLocation))
            {
                while (true)
                {
                    string linedata = sr.ReadLine();
                    string[] temp = new string[2];
                    if (linedata == null)
                        break;
                    else
                    {
                        temp = linedata.Split('=');
                        switch (temp[0].ToLower())
                        {
                            case "wpa":
                                {
                                    if (temp[1].ToString() != "")
                                    {
                                        wpa = temp[1].ToString();
                                    }
                                    break;
                                }
                            case "path":
                            {
                                    if (temp[1].ToString() != "")
                                    {
                                        for (int x = 1; x < temp.Length; x++)
                                        {
                                            if (x == 1)
                                                SQLConnection += temp[x].ToString();
                                            else
                                                SQLConnection += "=" + temp[x].ToString();
                                        }

                                    }
                                    break;
                                }
                            case "sqlconnection":
                                {
                                    if (temp[1].ToString() != "")
                                    {
                                        for (int x = 1; x < temp.Length; x++)
                                        {
                                            if (x==1)
                                                SQLConnection += temp[x].ToString();
                                            else
                                                SQLConnection += "=" + temp[x].ToString();
                                        }
                                        
                                    }
                                    break;
                                }
                            case "nocommand":
                                {
                                    if (temp[1].ToString() != "")
                                    {
                                        if (temp[1].Contains(","))
                                        {
                                            disregard = temp[1].Split(',');
                                        }
                                    }
                                    break;
                                }                           
                            case "username":
                                {
                                    if (temp[1].ToString() != "")
                                    {
                                        u_name = temp[1].ToString();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Please enter your username.  If you have not done so, set up RPC user/password/port/allowip in reddcoin.config file.");
                                        Errors.errors += "Please enter your username.  If you have not done so, set up RPC user/password/port/allowip in reddcoin.config file.";
                                        u_name = null;
                                    }
                                    break;
                                }
                            case "password":
                                {
                                    if (temp[1].ToString() != "")
                                    {
                                        p_word = temp[1].ToString();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Please enter your password.  If you have not done so, set up RPC user/password/port/allowip in reddcoin.config file.");
                                        Errors.errors += "Please enter your password.  If you have not done so, set up RPC user/password/port/allowip in reddcoin.config file.";
                                        p_word = null;
                                    }
                                    break;
                                }
                            case "ip_addr":
                                {
                                    if (temp[1].ToString() != "")
                                    {
                                        ip = temp[1].ToString();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Please enter your ip address.  If you have not done so, set up RPC user/password/port/allowip in reddcoin.config file.");
                                        Errors.errors += "Please enter your ip address.  If you have not done so, set up RPC user/password/port/allowip in reddcoin.config file.";
                                        ip = null;
                                    }
                                    break;
                                }
                            case "port":
                                {
                                    if (temp[1].ToString() != "")
                                    {
                                        port = temp[1].ToString();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Please enter your port.  If you have not done so, set up RPC user/password/port/allowip in reddcoin.config file.");   
                                        Errors.errors += ("Please enter your port.  If you have not done so, set up RPC user/password/port/allowip in reddcoin.config file.");
                                        port = null;
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            if (u_name != null && p_word != null && ip != null && port != null)
                result = true;
            else
            {
                result = false;
                Console.WriteLine(Errors.errors);
                Console.WriteLine("Press Any Key To Continue...");
                Console.ReadKey();
            }
            return result;
        }
    }

    public static class Parse
    {
        private static string missingdata = "Missing parameters.  Please see help guide for instructions on use.";
        private static string toomuchdata = "Too many parameters for this function.  Please try again.";
        private static string nodataneeded = "No parameters are needed for this function.";

        public static string Parse_cmd(string cmd, string[] param)
        {
            string data = string.Empty;
            bool valid_command = true;
            if (Load.disregard != null)
            {
                foreach (string s in Load.disregard)
                {
                    if (cmd.ToLower() == s.ToLower())
                    {
                        valid_command = false;
                        break;
                    }
                }
            }
            if (valid_command)
            {
                switch (cmd.ToLower())
                {
                    default:
                        {
                            data = "Command " + cmd + " is not a valid command.  Please type help for a list of valid commands, or google it.";
                            break;
                        }
                    case "addmultisigaddress":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 3:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                                data = RPC.addmultisigaddress(Convert.ToInt32(param[0]), param[1], param[2]);
                                            else
                                                data = "First Parameter required must be an integer > 0";
                                            break;
                                        }
                                    case 2:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                                data = RPC.addmultisigaddress(Convert.ToInt32(param[0]), param[1]);
                                            else
                                                data = "First Parameter required must be an integer > 0";
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "addnode":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            data = RPC.addnode(param[0], param[1]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "backupwallet":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.backupwallet(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "createmultisig":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                                data = RPC.createmultisig(Convert.ToInt32(param[0]), param[1]);
                                            else
                                                data = "First Parameter must be an integer > 0";
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "createrawtransaction":
                        {
                            if (param != null)
                            {

                            }
                            else
                            {

                            }
                           
                            break;
                        }
                    case "dumpprivkey":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.dumpprivkey(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "dumpwallet":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.dumpwallet(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getaccount":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.getaccount(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getaccountaddress":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.getaccountaddress(param[0]);
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getaddednodeinfo":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.getaddednodeinfo(param[0]);
                                            break;
                                        }
                                    case 2:
                                        {
                                            data = RPC.getaddednodeinfo(param[0], param[1]);
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getaddressesbyaccount":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.getaddressesbyaccount(param[0]);
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getbalance":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.getbalance(param[0]);
                                            break;
                                        }
                                    case 2:
                                        {
                                            int val;
                                            if (int.TryParse(param[1], out val))
                                                data = RPC.getbalance(param[0], Convert.ToInt32(param[1]));
                                            else
                                                data = "Second Parameter must be an integer > 0";
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.getbalance();
                            }
                            break;
                        }
                    case "getbestblockhash":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getbestblockhash();
                            }
                            break;
                        }
                    case "getblock":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            bool val;
                                            if (bool.TryParse(param[1], out val))
                                                data = RPC.getblock(param[0], Convert.ToBoolean(param[1]));
                                            else
                                                data = "Second Parameter must be \"true\" or \"false\"";
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getblockchaininfo":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getblockchaininfo();
                            }
                            break;
                        }
                    case "getblockcount":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getblockcount();
                            }
                            break;
                        }

                    case "getblockhash":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            int value;
                                            if (int.TryParse(param[0], out value))
                                                data = RPC.getblockhash(Convert.ToInt32(param[0]));
                                            else
                                                data = "Please use a whole number (integer).";
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }

                    case "getconnectioncount":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getconnectioncount();
                            }
                            break;
                        }
                    case "getdifficulty":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getdifficulty();
                            }
                            break;
                        }
                    case "getgenerate":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getgenerate();
                            }
                            break;
                        }
                    case "gethashespersec":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.gethashespersec();
                            }
                            break;
                        }
                    case "getinfo":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getinfo();
                            }
                            break;
                        }
                    case "getinterest":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int value;
                                            if (int.TryParse(param[0], out value))
                                            {
                                                if (int.TryParse(param[1], out value))
                                                {
                                                    data = RPC.getinterest(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
                                                }
                                                data += "Second parameter is not a UNIX timestamp (really long integer)";
                                            }
                                            data = "First parameter is not a UNIX timestamp (really long integer)";

                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getmininginfo":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getmininginfo();
                            }
                            break;
                        }
                    case "getnettotals":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getnettotals();
                            }
                            break;
                        }
                    case "getnetworkhashps":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int value;
                                            if (int.TryParse(param[0], out value))
                                            {
                                                if (int.TryParse(param[1], out value))

                                                    data = RPC.getnetworkhashps(Convert.ToInt32(param[0], Convert.ToInt32(param[1])));
                                                else
                                                    data = "Second Parameter not an integer, however...\r\n" + RPC.getnetworkhashps(Convert.ToInt32(param[0]));
                                            }
                                            else
                                            {
                                                data = "The first parameter is not an integer";
                                            }
                                            break;
                                        }
                                    case 1:
                                        {
                                            int value;
                                            if (int.TryParse(param[0], out value))
                                            {
                                                data = RPC.getnetworkhashps(Convert.ToInt32(param[0]));
                                            }
                                            else
                                            {
                                                data = "The first parameter is not an integer";
                                            }
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getnetworkinfo":
                        {
                            if (param != null)
                            {
                                data = nodataneeded;
                            }
                            else
                            {
                                data = RPC.getnetworkinfo();
                            }
                            break;
                        }
                    case "getnewaddress":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.getnewaddress(param[1]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getpeerinfo":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.getpeerinfo();
                            }
                            break;
                        }
                    case "getrawchangeaddress":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.getrawchangeaddress();
                            }
                            break;
                        }
                    case "getrawmempool":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            bool value;
                                            if (bool.TryParse(param[0], out value))
                                            {
                                                data = RPC.getrawmempool(Convert.ToBoolean(param[0]));
                                            }
                                            else
                                                data = "Please use true or false";
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getrawtransaction":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            bool value;
                                            if (bool.TryParse(param[1], out value))
                                                data = RPC.getrawtransaction(param[0], Convert.ToBoolean(param[1]));
                                            else
                                                data = "Requires a boolean argument (true, false) for last parameter.";
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.getrawtransaction(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getreceivedbyaccount":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int value;
                                            if (int.TryParse(param[1], out value))
                                                data = RPC.getreceivedbyaccount(param[0], Convert.ToInt32(param[1]));
                                            else
                                                data = "The last parameter is not an integer.";
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.getreceivedbyaccount(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getreceivedbyaddress":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int value;
                                            if (int.TryParse(param[1], out value))
                                                data = RPC.getreceivedbyaddress(param[0], Convert.ToInt32(param[1]));
                                            else
                                                data = "The last parameter is not an integer.";
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.getreceivedbyaddress(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "getstakinginfo":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.getstakinginfo();
                            }
                            break;
                        }
                    case "gettransaction":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.gettransaction(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "gettxout":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 3:
                                        {
                                            int value;
                                            bool b_value;
                                            if (int.TryParse(param[1], out value) && bool.TryParse(param[2], out b_value))
                                            {
                                                data = RPC.gettxout(param[0], Convert.ToInt32(param[1]), Convert.ToBoolean(param[2]));
                                            }
                                            else
                                                data = "Parameters required:  TX_ID (string), n (int), (optional includemempool [bool]).";
                                            break;
                                        }
                                    case 2:
                                        {
                                            int value;
                                            if (int.TryParse(param[1], out value))
                                                data = RPC.gettxout(param[0], Convert.ToInt32(param[1]));
                                            else
                                                data = "Second Parameter is not an integer.";
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "gettxoutsetinfo":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.gettxoutsetinfo();
                            }
                            break;
                        }
                    case "getunconfirmedbalance":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.getunconfirmedbalance();
                            }
                            break;
                        }
                    case "getwalletinfo":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.getwalletinfo();
                            }
                            break;
                        }
                    case "getwork":
                        {
                            data = "Depreciated.  No more POW blocks (code -1)";
                            break;
                        }
                    case "help":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.help(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.help();
                            }

                            break;
                        }
                    case "importprivkey":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 3:
                                        {
                                            bool val;
                                            if (bool.TryParse(param[2], out val))
                                                data = RPC.importprivkey(param[0], param[1], Convert.ToBoolean(param[2]));
                                            else
                                                data = "Last parameter is not a boolean value...Please use True or False";
                                            break;
                                        }
                                    case 2:
                                        {
                                            data = RPC.importprivkey(param[0], param[1]);
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.importprivkey(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "importwallet":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.importwallet(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "keypoolrefill":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                                data = RPC.keypoolrefill(Convert.ToInt32(param[0]));
                                            else
                                                data = "Parameter is not an integer";
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.keypoolrefill();
                            }
                            break;
                        }
                    case "listaccounts":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                            {
                                                data = RPC.listaccounts(Convert.ToInt32(param[0]));
                                            }
                                            else
                                                data = "Parameter is not an integer.";
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.listaccounts();
                            }
                            break;
                        }
                    case "listaddressgroupings":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.listaddressgroupings();
                            }
                            break;
                        }
                    case "listlockunspent":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.listlockunspent();
                            }
                            break;
                        }
                    case "listreceivedbyaccount":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int val;
                                            bool bval;
                                            if (int.TryParse(param[0], out val) && bool.TryParse(param[1], out bval))
                                                data = RPC.listreceivedbyaccount(Convert.ToInt32(param[0]), Convert.ToBoolean(param[1]));
                                            else
                                                data = "Expecting Int, then Bool.  Received " + param[0].GetType() + ":" + param[1].GetType();
                                            break;
                                        }
                                    case 1:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                                data = RPC.listreceivedbyaccount(Convert.ToInt32(param[0]));
                                            else
                                                data = "Integer required.  Type is " + param[0].GetType();
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "listreceivedbyaddress":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int val;
                                            bool bval;
                                            if (int.TryParse(param[0], out val) && bool.TryParse(param[1], out bval))
                                                data = RPC.listreceivedbyaddress(Convert.ToInt32(param[0]), Convert.ToBoolean(param[1]));
                                            else
                                                data = "Expecting Int, then Bool.  Received " + param[0].GetType() + ":" + param[1].GetType();
                                            break;
                                        }
                                    case 1:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                                data = RPC.listreceivedbyaddress(Convert.ToInt32(param[0]));
                                            else
                                                data = "Integer required.  Type is " + param[0].GetType();
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "listsinceblock":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int val;
                                            if (int.TryParse(param[1], out val))
                                            {
                                                data = RPC.listsinceblock(param[0], Convert.ToInt32(param[1]));
                                            }
                                            else
                                                data = "Expected int received " + param[1].GetType();
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.listsinceblock(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.listsinceblock();
                            }
                            break;
                        }
                    case "listtransactions":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 3:
                                        {
                                            int val;
                                            if (int.TryParse(param[1], out val) && int.TryParse(param[2], out val))
                                            {
                                                if (Convert.ToInt32(param[1]) > 0 && Convert.ToInt32(param[2]) > 0)
                                                {
                                                    data = RPC.listtransactions(param[0], Convert.ToInt32(param[1]), Convert.ToInt32(param[2]));
                                                }
                                                else
                                                {
                                                    data = "Requires two integers greater than 0";
                                                }
                                            }
                                            else
                                            {
                                                data = "Requires two integers greater than 0";
                                            }
                                            break;
                                        }
                                    case 2:
                                        {
                                            int val;
                                            if (int.TryParse(param[1], out val))
                                                if (Convert.ToInt32(param[1]) > 0)
                                                    data = RPC.listtransactions(param[0], Convert.ToInt32(param[1]));
                                                else
                                                    data = "Number must be greater than 0";
                                            else
                                                data = "Is not an integer";
                                            break;
                                        }
                                    case 1:
                                        {
                                            data = RPC.listtransactions(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.listtransactions();
                            }
                            break;
                        }
                    case "listunspent":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 3:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val) && int.TryParse(param[1], out val))
                                                if (Convert.ToInt32(param[0]) > 0 && Convert.ToInt32(param[1]) > 0)
                                                    data = RPC.listunspent(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]), param[2]);
                                                else
                                                    data = "Requires integers that are greater than 0";
                                            else
                                                data = "Requires integers that are greater than 0";
                                            break;
                                        }
                                    case 2:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val) && int.TryParse(param[1], out val))
                                                if (Convert.ToInt32(param[0]) > 0 && Convert.ToInt32(param[1]) > 0)
                                                    data = RPC.listunspent(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
                                                else
                                                    data = "Requires integers that are greater than 0";
                                            else
                                                data = "Requires integers that are greater than 0";
                                            break;
                                        }
                                    case 1:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                                data = RPC.listunspent(Convert.ToInt32(param[0]));
                                            else
                                                data = "Requires an integer for min confirmations.";
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.listunspent();
                            }
                            break;
                        }
                    case "move":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 5:
                                        {
                                            decimal val;
                                            int minval;
                                            if (decimal.TryParse(param[2], out val) && int.TryParse(param[3], out minval))
                                                data = RPC.move(param[0], param[1], Convert.ToDecimal(param[2]), Convert.ToInt32(param[3]), param[4]);
                                            else
                                                data = "Please use [string]from, [string]to, [decimal]amount";
                                            break;
                                        }
                                    case 4:
                                        {
                                            decimal val;
                                            int minval;
                                            if (decimal.TryParse(param[2], out val) && int.TryParse(param[3], out minval))
                                                data = RPC.move(param[0], param[1], Convert.ToDecimal(param[2]), Convert.ToInt32(param[3]));
                                            else
                                                data = "Please use [string]from, [string]to, [decimal]amount";
                                            break;
                                        }
                                    case 3:
                                        {
                                            decimal val;
                                            if (decimal.TryParse(param[2], out val))
                                                data = RPC.move(param[0], param[1], Convert.ToDecimal(param[2]));
                                            else
                                                data = "Please use [string]from, [string]to, [decimal]amount";
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 4)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                            }
                            break;
                        }
                    case "ping":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.ping();
                            }
                            break;
                        }
                    case "reservebalance":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            bool bval;
                                            decimal dval;
                                            if (bool.TryParse(param[0], out bval) && decimal.TryParse(param[1], out dval))
                                                data = RPC.reservebalance(Convert.ToBoolean(param[0]), Convert.ToDecimal(param[1]));
                                            else
                                                data = "Must use either True or False, and a coin amount is required";
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 2)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.reservebalance();
                            }
                            break;
                        }
                    case "sendfrom":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 6:
                                    case 5:
                                    case 4:
                                        {
                                            decimal val;
                                            if (decimal.TryParse(param[2], out val))
                                                data = RPC.sendfrom(param[0], param[1], Convert.ToDecimal(param[2]));
                                            else
                                                data = "Please use amount > 0";
                                            break;
                                        }
                                    case 3:
                                        {
                                            decimal val;
                                            if (decimal.TryParse(param[2], out val))
                                                data = RPC.sendfrom(param[0], param[1], Convert.ToDecimal(param[2]));
                                            else
                                                data = "Please use amount > 0";
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 6)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "sendmany":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 4:
                                        {
                                            if (param[1].Contains(":"))
                                            {
                                                int val;
                                                if (int.TryParse(param[2], out val))
                                                    data = RPC.sendmany(param[0], param[1], Convert.ToInt32(param[2]), param[3]);
                                                else
                                                    data = "Min Confirmations must be an int > 0";
                                            }
                                            else
                                                data = "The send to should be \"RDD ADDRESS\":Amount\r\nTo do multiple send tos, use \"RDD ADDRESS\":amount,\"RDD ADDRESS\":Amount, etc";
                                            break;
                                        }
                                    case 3:
                                        {
                                            if (param[1].Contains(":"))
                                            {
                                                int val;
                                                if (int.TryParse(param[2], out val))
                                                    data = RPC.sendmany(param[0], param[1], Convert.ToInt32(param[2]));
                                                else
                                                    data = "Min Confirmations must be an int > 0";
                                            }
                                            else
                                                data = "The send to should be \"RDD ADDRESS\":Amount\r\nTo do multiple send tos, use \"RDD ADDRESS\":amount,\"RDD ADDRESS\":Amount, etc";
                                            break;
                                        }
                                    case 2:
                                        {
                                            if (param[1].Contains(":"))
                                                data = RPC.sendmany(param[0], param[1]);
                                            else
                                                data = "The send to should be \"RDD ADDRESS\":Amount\r\nTo do multiple send tos, use \"RDD ADDRESS\":amount,\"RDD ADDRESS\":Amount, etc";
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 4)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "sendtoaddress":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 4:
                                        {
                                            decimal val;
                                            if (decimal.TryParse(param[1], out val))
                                                data = RPC.sendtoaddress(param[1], Convert.ToDecimal(param[1]), param[2], param[3]);
                                            else
                                                data = "Amount is required.  Must be decimal > 0";
                                            break;
                                        }
                                    case 3:
                                        {
                                            decimal val;
                                            if (decimal.TryParse(param[1], out val))
                                                data = RPC.sendtoaddress(param[1], Convert.ToDecimal(param[1]), param[2]);
                                            else
                                                data = "Amount is required.  Must be decimal > 0";
                                            break;
                                        }
                                    case 2:
                                        {
                                            decimal val;
                                            if (decimal.TryParse(param[1], out val))
                                                data = RPC.sendtoaddress(param[1], Convert.ToDecimal(param[1]));
                                            else
                                                data = "Amount is required.  Must be decimal > 0";
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 4)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "setaccount":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            data = RPC.setaccount(param[0], param[1]);
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 2)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }

                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "setgenerate":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            bool val;
                                            int ival;
                                            if (bool.TryParse(param[0], out val))
                                                if (int.TryParse(param[1], out ival))
                                                    data = RPC.setgenerate(Convert.ToBoolean(param[0]), Convert.ToInt32(param[1]));
                                                else
                                                    data = "Must use an integer.  -1 is unlimited.";
                                            else
                                                data = "Must be boolean value.";
                                            break;
                                        }
                                    case 1:
                                        {
                                            bool val;
                                            if (bool.TryParse(param[0], out val))
                                                data = RPC.setgenerate(Convert.ToBoolean(param[0]));
                                            else
                                                data = "Must be boolean value.";
                                            break;
                                        }
                                    default:
                                        {
                                            data = toomuchdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "settxfee":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            decimal val;
                                            if (decimal.TryParse(param[0], out val))
                                                data = RPC.settxfee(Convert.ToDecimal(param[0]));
                                            else
                                                data = "Must be a decimal.";
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 1)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "signmessage":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            data = RPC.signmessage(param[0], param[1]);
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 2)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "stop":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                RPC.stop();
                            }
                            break;
                        }
                    case "validateaddress":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 1:
                                        {
                                            data = RPC.validateaddress(param[0]);
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 1)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "verifychain":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val) && int.TryParse(param[1], out val))
                                                data = RPC.verifychain(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
                                            else
                                                data = "One or more parameters are not an integer.";
                                            break;
                                        }
                                    case 1:
                                        {
                                            int val;
                                            if (int.TryParse(param[0], out val))
                                                data = RPC.verifychain(Convert.ToInt32(param[0]));
                                            else
                                                data = "Parameter is not an integer.";
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 3)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = RPC.verifychain();
                            }
                            break;
                        }
                    case "verifymessage":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 3:
                                        {
                                            data = RPC.verifymessage(param[0], param[1], param[2]);
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 3)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "walletlock":
                        {
                            if (param != null)
                            {
                                data = toomuchdata;
                            }
                            else
                            {
                                data = RPC.walletlock();
                            }
                            break;
                        }
                    case "walletpassphrase":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 3:
                                        {
                                            int val;
                                            bool b_val;
                                            if (int.TryParse(param[1], out val) && bool.TryParse(param[2], out b_val))
                                                data = RPC.walletpassphrase(param[0], Convert.ToInt32(param[1]), Convert.ToBoolean(param[2]));
                                            else
                                                data = "Timeout must be an integer > 0 and staking parameter must be \"true\" or \"false\"";
                                            break;
                                        }
                                    case 2:
                                        {
                                            int val;
                                            if (int.TryParse(param[1], out val))
                                                data = RPC.walletpassphrase(param[0], Convert.ToInt32(param[1]));
                                            else
                                                data = "Timeout must be an integer > 0";
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 3)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                    case "walletpassphrasechange":
                        {
                            if (param != null)
                            {
                                switch (param.Length)
                                {
                                    case 2:
                                        {
                                            data = RPC.walletpassphrasechange(param[0], param[1]);
                                            break;
                                        }
                                    default:
                                        {
                                            if (param.Length > 2)
                                                data = toomuchdata;
                                            else
                                                data = missingdata;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                data = missingdata;
                            }
                            break;
                        }
                }
            }
            else data = "This command has been invalidated.  Please contact tech support for more help";
            return data;
        }
    }

    public static class RPC
    {
        #region addmultisigaddress

        public static string addmultisigaddress(int nrequired, string keys, string account)
        {
            string data = string.Empty;
            string[] a_keys = new string[1];
            if (keys.Contains(","))
            {
                a_keys = new string[keys.Split(',').Length];
                a_keys = keys.Split(',');
            }
            else
                a_keys[0] = keys;
            if (nrequired == a_keys.Length)
            {
                try
                {
                    JArray arrayKeys = JArray.FromObject(a_keys);
                    string result = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                    request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                    request.ContentType = "application/json-rpc";
                    request.Method = "POST";
                    JObject joe = new JObject();
                    joe.Add(new JProperty("jsonrpc", "1.0"));
                    joe.Add(new JProperty("id", "1"));
                    joe.Add(new JProperty("method", "addmultisigaddress"));
                    joe.Add(new JProperty("params", new JArray(nrequired, arrayKeys, account)));
                    string s = JsonConvert.SerializeObject(joe);
                    byte[] byteArray = Encoding.UTF8.GetBytes(s);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        data = JsonConvert.DeserializeObject(result).ToString();
                    }
                }
                catch (Exception ex)
                {
                    data = ex.ToString();
                }
            }
            else
            {
                data = "The number of Addresses is not the same as the required addresses.";
            }
            return data;
        }

        public static string addmultisigaddress(int nrequired, string keys)
        {
            string data = string.Empty;
            string[] a_keys = new string[1];
            if (keys.Contains(","))
            {
                a_keys = new string[keys.Split(',').Length];
                a_keys = keys.Split(',');
            }
            else
                a_keys[0] = keys;
            if (nrequired == a_keys.Length)
            {
                try
                {
                    JArray arrayKeys = JArray.FromObject(a_keys);
                    string result = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                    request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                    request.ContentType = "application/json-rpc";
                    request.Method = "POST";
                    JObject joe = new JObject();
                    joe.Add(new JProperty("jsonrpc", "1.0"));
                    joe.Add(new JProperty("id", "1"));
                    joe.Add(new JProperty("method", "addmultisigaddress"));
                    joe.Add(new JProperty("params", new JArray(nrequired, arrayKeys)));
                    string s = JsonConvert.SerializeObject(joe);
                    byte[] byteArray = Encoding.UTF8.GetBytes(s);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        data = JsonConvert.DeserializeObject(result).ToString();
                    }
                }
                catch (Exception ex)
                {
                    data = ex.ToString();
                }
            }
            else
                data = "The number of Addresses is not the same as the required addresses.";
            return data;
        }

        #endregion addmultisigaddress

        public static string addnode(string node, string type)
        {
            string data = string.Empty;
            if (type.ToLower() == "add" || type.ToLower() == "remove" || type.ToLower() == "onetry")
            {
                try
                {
                    string result = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                    request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                    request.ContentType = "application/json-rpc";
                    request.Method = "POST";
                    JObject joe = new JObject();
                    joe.Add(new JProperty("jsonrpc", "1.0"));
                    joe.Add(new JProperty("id", "1"));
                    joe.Add(new JProperty("method", "addnode"));
                    joe.Add(new JProperty("params", new JArray(node, type)));
                    string s = JsonConvert.SerializeObject(joe);
                    byte[] byteArray = Encoding.UTF8.GetBytes(s);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        data = JsonConvert.DeserializeObject(result).ToString();
                    }
                }
                catch (Exception ex)
                {
                    data = ex.ToString();
                }
            }
            else
            {
                data = "Must use \"Add|Remove|OneTry\"";
            }
            return data;
        }

        public static string backupwallet(string destination)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "backupwallet"));
                joe.Add(new JProperty("params", new JArray(destination)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string createmultisig(int nrequired, string keys)
        {
            string data = string.Empty;
            string[] a_keys = new string[1];
            if (keys.Contains(","))
            {
                a_keys = new string[keys.Split(',').Length];
                a_keys = keys.Split(',');
            }
            else
                a_keys[0] = keys;
            if (nrequired == a_keys.Length)
            {
                try
                {
                    JArray arrayKeys = JArray.FromObject(a_keys);
                    string result = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                    request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                    request.ContentType = "application/json-rpc";
                    request.Method = "POST";
                    JObject joe = new JObject();
                    joe.Add(new JProperty("jsonrpc", "1.0"));
                    joe.Add(new JProperty("id", "1"));
                    joe.Add(new JProperty("method", "createmultisig"));
                    joe.Add(new JProperty("params", new JArray(nrequired, arrayKeys)));
                    string s = JsonConvert.SerializeObject(joe);
                    byte[] byteArray = Encoding.UTF8.GetBytes(s);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        data = JsonConvert.DeserializeObject(result).ToString();
                    }
                }
                catch (Exception ex)
                {
                    data = ex.ToString();
                }
            }
            else
                data = "The Key Count and specified keys do not match...\r\nYou have " + nrequired.ToString() + " keys required but only supplied " + a_keys.Length + " addresses.";
            return data;
        }

        /*public static string createrawtransaction(string txid, string keys)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "createmultisig"));
                joe.Add(new JProperty("params", new JArray(txid, keys)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }*/

        public static string walletpassphrase(string passphrase, int timeout, bool stakeonly)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "walletpassphrase"));
                joe.Add(new JProperty("params", new JArray(passphrase, timeout, stakeonly)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }
        public static string walletpassphrase(string passphrase, int timeout)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "walletpassphrase"));
                joe.Add(new JProperty("params", new JArray(passphrase, timeout)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string dumpprivkey(string address)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "dumpprivkey"));
                joe.Add(new JProperty("params", new JArray(address)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string dumpwallet(string filename)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "dumpwallet"));
                joe.Add(new JProperty("params", new JArray(filename)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getaccount(string address)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getaccount"));
                joe.Add(new JProperty("params", new JArray(address)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getaccountaddress(string account)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getaccountaddress"));
                joe.Add(new JProperty("params", new JArray(account)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region GetNewAddress

        public static string getnewaddress()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getnewaddress"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getnewaddress(string addr_id)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getnewaddress"));
                joe.Add(new JProperty("params", new JArray(addr_id)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion GetNewAddress

        #region getaddednodeinfo

        public static string getaddednodeinfo(string dns, string node)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getaddednodeinfo"));
                joe.Add(new JProperty("params", new JArray(dns, node)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getaddednodeinfo(string dns)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getaddednodeinfo"));
                joe.Add(new JProperty("params", new JArray(dns)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion getaddednodeinfo

        public static string getaddressesbyaccount(string account)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getaddressesbyaccount"));
                joe.Add(new JProperty("params", new JArray(account)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region getbalance

        public static string getbalance(string account, int minconf)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getbalance"));
                joe.Add(new JProperty("params", new JArray(account, minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getbalance(string account)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getbalance"));
                joe.Add(new JProperty("params", new JArray(account)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getbalance()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getbalance"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion getbalance

        public static string getbestblockhash()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getnewaddress"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region GetBlock

        public static string getblock(string hash, bool verbose)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getblock"));
                joe.Add(new JProperty("params", new JArray(hash, verbose)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getblock(string hash)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getblock"));
                joe.Add(new JProperty("params", new JArray(hash)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion GetBlock

        public static string getblockchaininfo()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getblockchaininfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getblockcount()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getblockcount"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getblockhash(int index)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getblockhash"));
                joe.Add(new JProperty("params", new JArray(index)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getconnectioncount()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getconnectioncount"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getdifficulty()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getdifficulty"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getgenerate()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getgenerate"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string gethashespersec()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "gethashespersec"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getinfo()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getinfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getinterest(int start, int stop)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getinterest"));
                joe.Add(new JProperty("params", new JArray(start, stop)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getmininginfo()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getmininginfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getnettotals()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getnettotals"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region GetNetworkHashPS

        public static string getnetworkhashps(int block, int height)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getnetworkhashps"));
                joe.Add(new JProperty("params", new JArray(block, height)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getnetworkhashps(int block)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getnetworkhashps"));
                joe.Add(new JProperty("params", new JArray(block)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getnetworkhashps()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getnetworkhashps"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion GetNetworkHashPS

        public static string getnetworkinfo()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getnetworkinfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getpeerinfo()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getpeerinfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getrawchangeaddress()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getrawchangeaddress"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region getrawmempool

        public static string getrawmempool(bool verbose)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getrawmempool"));
                joe.Add(new JProperty("params", new JArray(verbose)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getrawmempool()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getrawmempool"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion getrawmempool

        #region GetRawTx

        public static string getrawtransaction(string txid, bool verbose)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getrawtransaction"));
                joe.Add(new JProperty("params", new JArray(txid, verbose)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getrawtransaction(string txid)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getrawtransaction"));
                joe.Add(new JProperty("params", new JArray(txid)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion GetRawTx

        #region getreceivedbyaccount

        public static string getreceivedbyaccount(string account, int minconf)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getreceivedbyaccount"));
                joe.Add(new JProperty("params", new JArray(account, minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getreceivedbyaccount(string account)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getreceivedbyaccount"));
                joe.Add(new JProperty("params", new JArray(account)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion getreceivedbyaccount

        #region getreceivedbyaddress

        public static string getreceivedbyaddress(string address, int minconf)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getreceivedbyaddress"));
                joe.Add(new JProperty("params", new JArray(address, minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getreceivedbyaddress(string address)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getreceivedbyaddress"));
                joe.Add(new JProperty("params", new JArray(address)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion getreceivedbyaddress

        public static string getstakinginfo()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getstakinginfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string gettransaction(string txid)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "gettransaction"));
                joe.Add(new JProperty("params", new JArray(txid)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region gettxout

        public static string gettxout(string txid, int n)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "gettxout"));
                joe.Add(new JProperty("params", new JArray(txid, n)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string gettxout(string txid, int n, bool includemempool)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "gettxout"));
                joe.Add(new JProperty("params", new JArray(txid, n, includemempool)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion gettxout

        public static string gettxoutsetinfo()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "gettxoutsetinfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getunconfirmedbalance()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getunconfirmedbalance"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string getwalletinfo()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getwalletinfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region Help

        public static string help()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "help"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = (reader.ReadToEnd());
                    data = JsonConvert.DeserializeObject(result).ToString();

                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string help(string command)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "help"));
                joe.Add(new JProperty("params", new JArray(command)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion Help

        #region importprivkey

        public static string importprivkey(string privkey, string label, bool rescan)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getwalletinfo"));
                joe.Add(new JProperty("params", new JArray(privkey, label, rescan)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string importprivkey(string privkey, string label)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getwalletinfo"));
                joe.Add(new JProperty("params", new JArray(privkey, label)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string importprivkey(string privkey)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getwalletinfo"));
                joe.Add(new JProperty("params", new JArray(privkey)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion importprivkey

        public static string importwallet(string filename)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "importwallet"));
                joe.Add(new JProperty("params", new JArray(filename)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region keypoolrefill

        public static string keypoolrefill(int size)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getwalletinfo"));
                joe.Add(new JProperty("params", new JArray(size)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string keypoolrefill()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "getwalletinfo"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion keypoolrefill

        #region ListAccounts

        public static string listaccounts(int minconf)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listaccounts"));
                joe.Add(new JProperty("params", new JArray(minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listaccounts()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listaccounts"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion ListAccounts

        public static string listaddressgroupings()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listaddressgroupings"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listlockunspent()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listlockunspent"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region listreceivedbyaccount

        public static string listreceivedbyaccount(int minconf, bool includeempty)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listreceivedbyaccount"));
                joe.Add(new JProperty("params", new JArray(minconf, includeempty)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listreceivedbyaccount(int minconf)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listreceivedbyaccount"));
                joe.Add(new JProperty("params", new JArray(minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listreceivedbyaccount()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listreceivedbyaccount"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion listreceivedbyaccount

        #region listreceivedbyaddress

        public static string listreceivedbyaddress(int minconf, bool includeempty)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listreceivedbyaddress"));
                joe.Add(new JProperty("params", new JArray(minconf, includeempty)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listreceivedbyaddress(int minconf)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listreceivedbyaddress"));
                joe.Add(new JProperty("params", new JArray(minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listreceivedbyaddress()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listreceivedbyaddress"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion listreceivedbyaddress

        #region listsinceblock

        public static string listsinceblock(string blockhash, int target_confs)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listsinceblock"));
                joe.Add(new JProperty("params", new JArray(blockhash, target_confs)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listsinceblock(string blockhash)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listsinceblock"));
                joe.Add(new JProperty("params", new JArray(blockhash)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listsinceblock()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listsinceblock"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion listsinceblock

        #region listtransactions

        public static string listtransactions(string account, int count, int from)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listtransactions"));
                joe.Add(new JProperty("params", new JArray(account, count, from)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listtransactions(string account)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listtransactions"));
                joe.Add(new JProperty("params", new JArray(account)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listtransactions(string account, int count)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listtransactions"));
                joe.Add(new JProperty("params", new JArray(account, count)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listtransactions()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listtransactions"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion listtransactions

        #region ListUnspent

        public static string listunspent(int min, int max, string addresses)
        {
            string data = string.Empty;
            string[] arrayAddresses = new string[1];
            if (addresses.Contains(","))
            {
                arrayAddresses = new string[addresses.Split(',').Length];
                arrayAddresses = addresses.Split(',');
            }
            else
                arrayAddresses[0] = addresses;
            try
            {
                JArray AddressArray = JArray.FromObject(arrayAddresses);
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listunspent"));
                joe.Add(new JProperty("params", new JArray(min, max, AddressArray)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listunspent(int min, int max)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listunspent"));
                joe.Add(new JProperty("params", new JArray(min, max)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listunspent(int min)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listunspent"));
                joe.Add(new JProperty("params", new JArray(min)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string listunspent()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "listunspent"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion ListUnspent

        #region Move

        public static string move(string fromaccount, string toaccount, decimal amount, int minconf, string comment)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "move"));
                joe.Add(new JProperty("params", new JArray(fromaccount, toaccount, amount, minconf, comment)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string move(string fromaccount, string toaccount, decimal amount, int minconf)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "move"));
                joe.Add(new JProperty("params", new JArray(fromaccount, toaccount, amount, minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string move(string fromaccount, string toaccount, decimal amount)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "move"));
                joe.Add(new JProperty("params", new JArray(fromaccount, toaccount, amount)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion Move

        public static string ping()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "ping"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region reservebalance

        public static string reservebalance(bool reserve, decimal amount)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "reservebalance"));
                joe.Add(new JProperty("params", new JArray(reserve, amount)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string reservebalance()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "reservebalance"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion reservebalance

        #region sendfrom

        public static string sendfrom(string from, string to, decimal amount, int minconf, string comment, string comment_to)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendfrom"));
                joe.Add(new JProperty("params", new JArray(from, to, amount, minconf, comment, comment_to)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string sendfrom(string from, string to, decimal amount)
        {
            /*string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendfrom"));
                joe.Add(new JProperty("params", new JArray(from, to, amount)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }*/
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendfrom"));
                joe.Add(new JProperty("params", new JArray(from, to, amount)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }

            return data;
        }

        public static string sendfrom(string from, string to, decimal amount, int minconf)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendfrom"));
                joe.Add(new JProperty("params", new JArray(from, to, amount, minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string sendfrom(string from, string to, decimal amount, int minconf, string comment)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendfrom"));
                joe.Add(new JProperty("params", new JArray(from, to, amount, minconf, comment)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion sendfrom

        #region SendMany

        public static string sendmany(string from, string to, int minconf, string comment)
        {
            string data = string.Empty;
            Dictionary<string, decimal> dic_to = new Dictionary<string, decimal>();
            if (to.Contains(",") && to.Contains(":"))
            {
                string[] array_to = new string[to.Split(',').Length];
                array_to = to.Split(',');
                foreach (string s in array_to)
                {
                    string[] addit = s.Split(':');
                    dic_to.Add(addit[0].ToString(), Convert.ToDecimal(addit[1]));
                }
            }
            else if (to.Contains(":") && !to.Contains(","))
            {
                string[] addit = to.Split(':');
                dic_to.Add(addit[0], Convert.ToDecimal(addit[1]));
            }
            
            try
            {
                JArray J_To = JArray.FromObject(dic_to);
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendmany"));
                joe.Add(new JProperty("params", new JArray(from, J_To, minconf, comment)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string sendmany(string from, string to, int minconf)
        {
            string data = string.Empty;
            Dictionary<string, decimal> dic_to = new Dictionary<string, decimal>();
            if (to.Contains(",") && to.Contains(":"))
            {
                string[] array_to = new string[to.Split(',').Length];
                array_to = to.Split(',');
                foreach (string s in array_to)
                {
                    string[] addit = s.Split(':');
                    dic_to.Add(addit[0].ToString(), Convert.ToDecimal(addit[1]));
                }
            }
            else if (to.Contains(":") && !to.Contains(","))
            {
                string[] addit = to.Split(':');
                dic_to.Add(addit[0], Convert.ToDecimal(addit[1]));
            }
            try
            {
                JArray J_To = JArray.FromObject(to);
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendmany"));
                joe.Add(new JProperty("params", new JArray(from, J_To, minconf)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string sendmany(string from, string to)
        {
            Dictionary<string, decimal> dic_to = new Dictionary<string, decimal>();
            if (to.Contains(",") && to.Contains(":"))
            {
                string[] array_to = new string[to.Split(',').Length];
                array_to = to.Split(',');
                foreach (string s in array_to)
                {
                    string[] addit = s.Split(':');
                    dic_to.Add(addit[0].ToString(), Convert.ToDecimal(addit[1]));
                }
            }
            else if (to.Contains(":") && !to.Contains(","))
            {
                string[] addit = to.Split(':');
                dic_to.Add(addit[0], Convert.ToDecimal(addit[1]));
            }
            string data = string.Empty;
            try
            {
                JArray J_To = JArray.FromObject(to);
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendmany"));
                joe.Add(new JProperty("params", new JArray(from, J_To)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion SendMany

        #region SendToAddress

        public static string sendtoaddress(string to, decimal amount, string comment, string comment_to)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendtoaddress"));
                joe.Add(new JProperty("params", new JArray(to, amount, comment, comment_to)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string sendtoaddress(string to, decimal amount, string comment)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendtoaddress"));
                joe.Add(new JProperty("params", new JArray(to, amount, comment)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string sendtoaddress(string to, decimal amount)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendtoaddress"));
                joe.Add(new JProperty("params", new JArray(to, amount)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion SendToAddress

        public static string setaccount(string address, string account)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "setaccount"));
                joe.Add(new JProperty("params", new JArray(address, account)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region SetGenerate

        public static string setgenerate(bool gen, int proclim)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "setgenerate"));
                joe.Add(new JProperty("params", new JArray(gen, proclim)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string setgenerate()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "setgenerate"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string setgenerate(bool gen)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "setgenerate"));
                joe.Add(new JProperty("params", new JArray(gen)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion SetGenerate

        public static string settxfee(decimal amount)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "settxfee"));
                joe.Add(new JProperty("params", new JArray(amount)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string signmessage(string address, string message)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "signmessage"));
                joe.Add(new JProperty("params", new JArray(address, message)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string stop()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "sendtoaddress"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string validateaddress(string address)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "validateaddress"));
                joe.Add(new JProperty("params", new JArray(address)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #region VerifyChain

        public static string verifychain(int lvl, int blocks)
        {
            bool b_lvl = false;
            bool b_blocks = false;

            if (lvl >= 0 && lvl <= 4)
                b_lvl = true;
            if (blocks > 0)
                b_blocks = true;
            string data = string.Empty;
            if (b_blocks && b_lvl)
            {
                try
                {
                    string result = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                    request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                    request.ContentType = "application/json-rpc";
                    request.Method = "POST";
                    JObject joe = new JObject();
                    joe.Add(new JProperty("jsonrpc", "1.0"));
                    joe.Add(new JProperty("id", "1"));
                    joe.Add(new JProperty("method", "verifychain"));
                    joe.Add(new JProperty("params", new JArray(lvl, blocks)));
                    string s = JsonConvert.SerializeObject(joe);
                    byte[] byteArray = Encoding.UTF8.GetBytes(s);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        data = JsonConvert.DeserializeObject(result).ToString();
                    }
                }
                catch (Exception ex)
                {
                    data = ex.ToString();
                }
            }
            else
            {
                data = "";
                if (!b_lvl)
                    data += "Level must be between 0 and 4\r\n";
                if (!b_blocks)
                    data += "Block count must be >=0";
            }
            return data;
        }

        public static string verifychain(int lvl)
        {
            bool b_lvl = false;
            if (lvl >= 0 && lvl <= 4)
                b_lvl = true;
            string data = string.Empty;
            if (b_lvl)
            {
                try
                {
                    string result = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                    request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                    request.ContentType = "application/json-rpc";
                    request.Method = "POST";
                    JObject joe = new JObject();
                    joe.Add(new JProperty("jsonrpc", "1.0"));
                    joe.Add(new JProperty("id", "1"));
                    joe.Add(new JProperty("method", "verifychain"));
                    joe.Add(new JProperty("params", new JArray(lvl)));
                    string s = JsonConvert.SerializeObject(joe);
                    byte[] byteArray = Encoding.UTF8.GetBytes(s);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        data = JsonConvert.DeserializeObject(result).ToString();
                    }
                }
                catch (Exception ex)
                {
                    data = ex.ToString();
                }
            }
            else
            {
                data = "Level must be between 0 and 4";
            }
            return data;
        }

        public static string verifychain()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "verifychain"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        #endregion VerifyChain

        public static string verifymessage(string address, string sig, string message)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "verifymessage"));
                joe.Add(new JProperty("params", new JArray(address, sig, message)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string walletlock()
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "walletlock"));
                joe.Add(new JProperty("params",null));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }

        public static string walletpassphrasechange(string oldpw, string newpw)
        {
            string data = string.Empty;
            try
            {
                string result = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Load.ip + ":" + Load.port);
                request.Credentials = new NetworkCredential(Load.u_name, Load.p_word);
                request.ContentType = "application/json-rpc";
                request.Method = "POST";
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", "walletpassphrasechange"));
                joe.Add(new JProperty("params", new JArray(oldpw, newpw)));
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject(result).ToString();
                }
            }
            catch (Exception ex)
            {
                data = ex.ToString();
            }
            return data;
        }
    }
}