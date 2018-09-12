using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class CompilerSymbolSwitch
{
    private class SymbolStatus
    {
        public string name;
        public bool oldValue;
    }

    private static List<SymbolStatus> symbols_;
    private static List<SymbolStatus> Symbols
    {
        get
        {
            if (null == symbols_)
            {
                symbols_ = new List<SymbolStatus>();
            }
            return symbols_;
        }
    }

    public static void Enable(string _symbolsString)
    {
        CompilerSymbolManager.Load();

        string[] symbols = _symbolsString.Split(';');
        foreach (string symbol in symbols)
        {
            SymbolStatus sym = new SymbolStatus();
            sym.name = symbol;
            sym.oldValue = CompilerSymbolManager.ActivateSymbol(symbol, true);
            Symbols.Add(sym);
            Debug.Log("ENABLE: " + sym.name + "="  + sym.oldValue);
        }

        CompilerSymbolManager.Save();
    }

    public static void Enable(string enabled, string disabled)
    {
        CompilerSymbolManager.Load();

        string[] symbols;
        
        // enabled
        symbols = enabled.Split(';');
        foreach (string symbol in symbols)
        {
            SymbolStatus sym = new SymbolStatus();
            sym.name = symbol;
            sym.oldValue = CompilerSymbolManager.ActivateSymbol(symbol, true);
            Symbols.Add(sym);
            Debug.Log("ENABLE: " + sym.name + "="  + sym.oldValue);
        }

        // disabled
        symbols = disabled.Split(';');
        foreach (string symbol in symbols)
        {
            SymbolStatus sym = new SymbolStatus();
            sym.name = symbol;
            sym.oldValue = CompilerSymbolManager.ActivateSymbol(symbol, false);
            Symbols.Add(sym);
            Debug.Log("DISABLE: " + sym.name + "="  + sym.oldValue);
        }

        CompilerSymbolManager.Save();
    }

    public static void Disable(string _symbolsString)
    {
        System.Threading.Thread.Sleep(500);

        CompilerSymbolManager.Load();

        string[] symbols = _symbolsString.Split(';');
        foreach (string symbol in symbols)
        {
            SymbolStatus sym = new SymbolStatus();
            sym.name = symbol;
            sym.oldValue = CompilerSymbolManager.ActivateSymbol(symbol, false);
            Symbols.Add(sym);
            Debug.Log("DISABLE: " + sym.name + "=" + sym.oldValue);
        }
        
        CompilerSymbolManager.Save();
    }

    public static void Rollback()
    {
        System.Threading.Thread.Sleep(500);

        CompilerSymbolManager.Load();

        foreach (SymbolStatus sym in Symbols)
        {
            CompilerSymbolManager.ActivateSymbol(sym.name, sym.oldValue);
            Debug.Log(sym.oldValue ? "ENABLE: " : "DISABLE: " + sym.name + "="  + sym.oldValue);
        }
        CompilerSymbolManager.Save();
        Symbols.Clear();
    }

}
