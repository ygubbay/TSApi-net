﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSWeb.Reports
{
    public class HtmlReport
    {

        
        public string HtmlOutput { get {

            return @"<HTML>
                        <HEAD>
                        <META HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=windows-1255'>
                        <META HTTP-EQUIV='Pragma' CONTENT='no-cache'>
                        <STYLE>
                        TABLE {border-collapse:collapse; border-width:1px; border-color:black;}
                        TD {border-collapse:collapse; border-width:1px; border-color:black;}
                        #tdline {height:17px}
                        #tdcompany {height:14px}
                        #tdheader {height:39px;}
                        #tdlogo {height:90px;width:90px}
                        #tdcaption {height:20px;width:100px}
                        #tdspace {height:22px}
                        .namecompany {color:black;font-family:helvetica;font-size:10px;text-decoration:Underline;}
                        .linecompany {color:black;font-family:helvetica;font-size:10px;}
                        .tableline {color:black;font-family:helvetica;font-size:12px;}
                        .tablecap {color:black;font-family:helvetica;font-size:12px;font-weight:bold;}
                        .capline {color:black;font-weight:bold;font-family:helvetica;font-size:16px;font-weight:bold;}
                        .headerline{color:black;font-weight:bold;font-family:helvetica;font-size:26px;font-weight:bold;}
                        .ppage {height:915px;width:670px;page-break-after:always;border-style:solid;border-width:0px;border-color:black;overflow:hidden}
                        .plastpage {height:915px;width:670px;page-break-after:;border-style:solid;border-width:0px;border-color:black;overflow:hidden}
                        </STYLE></HEAD>
                        <BODY><CENTER>
                        <DIV CLASS='plastpage' DIR='RTL'><BR>
                        <TABLE ALIGN='CENTER' width='90%'>
                        <TR>
                        <TD COLSPAN='2' ALIGN='CENTER' CLASS='headerline' ID='tdheader'>העתק</TD>
                        </TR>
                        <TR>
                        <TD width='50%' >
                        <TABLE width='100%' CELLSPACING=1>

                        <TR><TD width='50%' ALIGN='RIGHT' ID='tdlogo'><IMG SRC='http://msn.arkia.co.il/images/logo_arkia.gif' width='90' height='80'></TD><TD width='50%' ALIGN='RIGHT' ID='tdlogo'>&nbsp;</TD></TR>
                        </TABLE>
                        </TD>
                        <TD width='50%' >
                        <TABLE width='100%'>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='namecompany' ID='tdcompany' width='25%'>ארקיע קווי תעופה ישראליים בעמ</TD>
                        <TD ALIGN='RIGHT' CLASS='namecompany' ID='tdcompany' width='25%'>ארקיע קליק - אתר עברית</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>שדה דב</TD>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>שדה דב, ת.ד. 39301</TD>

                        </TR>

                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>ת.ד. 39301 , תל-אביב  61392</TD>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>תל אביב 61392 , ישראל</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>ישראל</TD>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'></TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>טל: <SPAN DIR='LTR'>+972-3-6903712</SPAN></TD>

                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>טל: <SPAN DIR='LTR'>+972-3-6903456</SPAN></TD>

                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>פקס: <SPAN DIR='LTR'>+972-3-6990261</SPAN></TD>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'>פקס: <SPAN DIR='LTR'>+972-3-6900801</SPAN></TD>
                        </TR>
                        <TR><TD ALIGN='RIGHT' CLASS='linecompany' ID='tdval' width='25%'><SPAN DIR='LTR'>Click_Chief@arkia.co.il :Email</SPAN></TD>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdcompany' width='25%'><SPAN DIR='LTR'>web@arkia.co.il :Email</SPAN></TD></TR>

                        </TABLE>
                        </TD>
                        </TR>
                        <TR>

                        <TD COLSPAN='2' ID='tdcompany' width='25%'><HR></TD>
                        </TR>
                        </TABLE>
                        <TABLE ALIGN='CENTER' width='90%'>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline' width='400'>עוסק מורשה מאוחד : 557647716</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>חשבונית מס / קבלה</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'> : </TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>1893015</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline' width='400'>&nbsp;</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>תאריך</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'> : </TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>21/02/2012</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline' width='400'>&nbsp;</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>דף</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'> : </TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>1</TD>

                        </TR>
                        </TABLE>
                        <TABLE ALIGN='CENTER' width='90%'>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline'>לכבוד</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>מר יהודה גבאי</TD>
                        </TR>

                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>319210738</TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>

                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>

                        <TR>
                        <TD ALIGN='CENTER' CLASS='capline' ID='tdline'>חשבונית מס / קבלה מספר 1893015</TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>

                        </TABLE>
                        <TABLE ALIGN='CENTER' width='90%'>
                        <TR>
                        <TD CLASS='tableline' ID='tdline' COLSPAN='2'>
                        <TABLE ALIGN='CENTER' width='100%' BORDER='1'>
                        <TR>
                        <TD ALIGN='CENTER' CLASS='tablecap' ID='tdline' width='40%'>הזמנה 4188674</TD>
                        <TD ALIGN='CENTER' CLASS='tablecap' ID='tdline' width='12%'>תאריך</TD>

                        <TD ALIGN='CENTER' CLASS='tablecap' ID='tdline' width='18%'>נוסעים</TD>
                        <TD ALIGN='CENTER' CLASS='tablecap' ID='tdline' width='14%'>0% מע'מ<BR>שח</TD>
                        <TD ALIGN='CENTER' CLASS='tablecap' ID='tdline' width='14%'>16% מע'מ<BR>שח</TD>

                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdline'>הזמנה , אקספרס, טיסה, מתל אביב לאילת</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>27/02/2012</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>מר יהודה גבאי</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>         179.00</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>           0.00</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdline'>הזמנה , אקספרס, טיסה, מתל אביב לאילת</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>27/02/2012</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>גב Efrat Gubbay</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>         179.00</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>           0.00</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdline'>הזמנה , אקספרס, טיסה, מאילת לתל אביב</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>27/02/2012</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>מר יהודה גבאי</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>         179.00</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>           0.00</TD>

                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='linecompany' ID='tdline'>הזמנה , אקספרס, טיסה, מאילת לתל אביב</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>27/02/2012</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>גב Efrat Gubbay</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>         179.00</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>           0.00</TD>
                        </TR>
                        </TABLE>

                        </TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline' width='51%'> </TD>
                        <TD CLASS='tableline' ID='tdline' width='%'>
                        <TABLE width='100%' BORDER='1'>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline' width='65%'>סה'כ 0% מע'מ</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline' width='35%'>         716.00 שח</TD>

                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline'>סה'כ 16% מע'מ</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>           0.00 שח</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline'>16% מע'מ</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>           0.00 שח</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline'>סה'כ לתשלום</TD>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>         716.00 שח</TD>

                        </TR>
                        </TABLE>
                        </TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline' >שולם&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline' > </TD>
                        <TD CLASS='tableline' ID='tdline'> </TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>ארקיע קליק - אתר עברית / אתר אינטרנט עברית</TD>

                        <TD CLASS='tableline' ID='tdline'> </TD>

                        </TR>
                        <TR>
                        <TD COLSPAN='2'>&nbsp;</TD>
                        </TR>
                        <TR>
                        <TD>
                        <TABLE width='100%' BORDER='1'>
                        <TR>
                        <TD ALIGN='CENTER' CLASS='tablecap' ID='tdline' WIDTH='50%'>תשלומים</TD>
                        <TD ALIGN='CENTER' CLASS='tablecap' ID='tdline' WIDTH='25%'>שולם</TD>
                        <TD ALIGN='CENTER' CLASS='tablecap' ID='tdline' WIDTH='25%'>זוכה</TD>
                        </TR>

                        <TR>

                        <TD  ALIGN='RIGHT' CLASS='tableline' ID='tdline'>כרטיס אשראי ישרכארט/מסטרכרד 3822 , רגיל (1)</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>         716.00 שח</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>         716.00 שח</TD>
                        </TR>
                        </TABLE>
                        </TD>
                        <TD>&nbsp;</TD>
                        </TR>
                        <TR>
                        <TD>
                        <TABLE width='100%' >

                        <TR>
                        <TD WIDTH='50%'>&nbsp;</TD>
                        <TD WIDTH='50%'>
                        <TABLE width='100%' BORDER='1'>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline' WIDTH='50%'>סה'כ</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline' WIDTH='50%'>         716.00 שח</TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tablecap' ID='tdline'>יתרה לתשלום</TD>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'>           0.00 שח</TD>

                        </TR>
                        </TABLE>
                        </TD>
                        <TD>
                        </TD>
                        </TR></TABLE>
                        </TD>
                        </TR>
                        </TABLE>
                        <TABLE ALIGN='CENTER' width='90%' ID='tableborder'>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>

                        <TR>

                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><HR></TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'></TD>
                        </TR>
                        <TR>

                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'></TD>
                        </TR>

                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'></TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'></TD>
                        </TR>
                        <TR>
                        <TD ALIGN='RIGHT' CLASS='tableline' ID='tdline'></TD>
                        </TR>
                        <TR>
                        <TD CLASS='tableline' ID='tdline'><BR></TD>
                        </TR>
                        </TABLE>
                        </DIV>

                        </CENTER></BODY>
                        </HTML>";

    }}
    }
}
