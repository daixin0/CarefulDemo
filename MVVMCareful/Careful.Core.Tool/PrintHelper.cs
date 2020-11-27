using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Careful.Tool
{
    public class PrintHelper
    {
        public static bool PrintVisual(FrameworkElement element,string printName)
        {
            var printDialog = new PrintDialog();
            SetPrintProperty(printDialog);
            var printQueue = SelectedPrintServer(printName);
            if (printQueue != null)
            {
                printDialog.PrintQueue = printQueue;
                printDialog.PrintVisual(element, string.Empty);
                return true;
            }
            return false;
        }

        private static PrintQueue SelectedPrintServer(string printerName)
        {
            try
            {
                var printers = PrinterSettings.InstalledPrinters;//获取本机上的所有打印机
                PrintServer printServer = null;

                foreach (string printer in printers)
                {
                    if (printer.Contains(printerName))
                        printServer = new PrintServer(PrintSystemDesiredAccess.AdministrateServer);
                }

                if (printServer == null) return null;//没有找到打印机服务器

                var printQueue = printServer.GetPrintQueue(printerName);
                return printQueue;
            }
            catch (Exception ex)
            {
                return null;//没有找到打印机
            }
        }

        private static void SetPrintProperty(PrintDialog printDialog, PageMediaSizeName pageSize = PageMediaSizeName.ISOA4, PageOrientation pageOrientation = PageOrientation.Portrait)
        {
            var printTicket = printDialog.PrintTicket;
            printTicket.PageMediaSize = new PageMediaSize(pageSize);//A4纸
            printTicket.PageOrientation = pageOrientation;//默认竖向打印
        }
    }
}