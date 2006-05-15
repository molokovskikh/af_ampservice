Imports System.Web.Services
Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.net.Mail


Namespace AMPService


    <System.Web.Services.WebService(Namespace:="AMPNameSpace")> <Microsoft.VisualBasic.ComClass()> _
    Public Class AMPService
        Inherits System.Web.Services.WebService
        Dim MyTrans As MySqlTransaction
        Dim UserName, FunctionName As String
        Dim StartTime As Date = Now()
        Private WithEvents dataColumn18 As System.Data.DataColumn
        Private WithEvents dataColumn19 As System.Data.DataColumn
        Private Const SQLHost As String = "sql.analit.net"
        ' Dim RowCount As Int32

#Region " Web Services Designer Generated Code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Web Services Designer.
            InitializeComponent()

            'Add your own initialization code after the InitializeComponent() call

        End Sub

        'Required by the Web Services Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Web Services Designer
        'It can be modified using the Web Services Designer.  
        'Do not modify it using the code editor.
        Public WithEvents MySelCmd As MySql.Data.MySqlClient.MySqlCommand
        Public WithEvents MyCn As MySql.Data.MySqlClient.MySqlConnection
        Private WithEvents MyDA As MySql.Data.MySqlClient.MySqlDataAdapter
        Public WithEvents MyDS As System.Data.DataSet
        Public WithEvents DataTable1 As System.Data.DataTable
        Public WithEvents DataColumn1 As System.Data.DataColumn
        Public WithEvents DataColumn2 As System.Data.DataColumn
        Public WithEvents DataColumn3 As System.Data.DataColumn
        Public WithEvents DataColumn4 As System.Data.DataColumn
        Public WithEvents DataColumn5 As System.Data.DataColumn
        Public WithEvents DataColumn6 As System.Data.DataColumn
        Public WithEvents DataColumn7 As System.Data.DataColumn
        Public WithEvents DataColumn8 As System.Data.DataColumn
        Public WithEvents DataColumn9 As System.Data.DataColumn
        Public WithEvents DataColumn10 As System.Data.DataColumn
        Public WithEvents DataColumn11 As System.Data.DataColumn
        Public WithEvents DataColumn12 As System.Data.DataColumn
        Public WithEvents DataColumn13 As System.Data.DataColumn
        Public WithEvents DataColumn14 As System.Data.DataColumn
        Public WithEvents DataColumn15 As System.Data.DataColumn
        Public WithEvents DataColumn16 As System.Data.DataColumn
        Public WithEvents DataColumn17 As System.Data.DataColumn
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.MySelCmd = New MySql.Data.MySqlClient.MySqlCommand
            Me.MyCn = New MySql.Data.MySqlClient.MySqlConnection
            Me.MyDA = New MySql.Data.MySqlClient.MySqlDataAdapter
            Me.MyDS = New System.Data.DataSet
            Me.DataTable1 = New System.Data.DataTable
            Me.DataColumn1 = New System.Data.DataColumn
            Me.DataColumn2 = New System.Data.DataColumn
            Me.DataColumn3 = New System.Data.DataColumn
            Me.DataColumn4 = New System.Data.DataColumn
            Me.DataColumn5 = New System.Data.DataColumn
            Me.DataColumn6 = New System.Data.DataColumn
            Me.DataColumn7 = New System.Data.DataColumn
            Me.DataColumn8 = New System.Data.DataColumn
            Me.DataColumn9 = New System.Data.DataColumn
            Me.DataColumn10 = New System.Data.DataColumn
            Me.DataColumn11 = New System.Data.DataColumn
            Me.DataColumn12 = New System.Data.DataColumn
            Me.DataColumn13 = New System.Data.DataColumn
            Me.DataColumn14 = New System.Data.DataColumn
            Me.DataColumn15 = New System.Data.DataColumn
            Me.DataColumn16 = New System.Data.DataColumn
            Me.DataColumn17 = New System.Data.DataColumn
            Me.dataColumn18 = New System.Data.DataColumn
            Me.dataColumn19 = New System.Data.DataColumn
            CType(Me.MyDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DataTable1, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'MySelCmd
            '
            Me.MySelCmd.CommandText = Nothing
            Me.MySelCmd.CommandTimeout = 0
            Me.MySelCmd.CommandType = System.Data.CommandType.Text
            Me.MySelCmd.Connection = Me.MyCn
            Me.MySelCmd.Transaction = Nothing
            Me.MySelCmd.UpdatedRowSource = System.Data.UpdateRowSource.Both
            '
            'MyCn
            '
            Me.MyCn.ConnectionString = ""
            '
            'MyDA
            '
            Me.MyDA.DeleteCommand = Nothing
            Me.MyDA.InsertCommand = Nothing
            Me.MyDA.SelectCommand = Me.MySelCmd
            Me.MyDA.UpdateCommand = Nothing
            '
            'MyDS
            '
            Me.MyDS.DataSetName = "AMPDataSet"
            Me.MyDS.Locale = New System.Globalization.CultureInfo("ru-RU")
            Me.MyDS.Tables.AddRange(New System.Data.DataTable() {Me.DataTable1})
            '
            'DataTable1
            '
            Me.DataTable1.Columns.AddRange(New System.Data.DataColumn() {Me.DataColumn1, Me.DataColumn2, Me.DataColumn3, Me.DataColumn4, Me.DataColumn5, Me.DataColumn6, Me.DataColumn7, Me.DataColumn8, Me.DataColumn9, Me.DataColumn10, Me.DataColumn11, Me.DataColumn12, Me.DataColumn13, Me.DataColumn14, Me.DataColumn15, Me.DataColumn16, Me.DataColumn17, Me.dataColumn18, Me.dataColumn19})
            Me.DataTable1.TableName = "Prices"
            '
            'DataColumn1
            '
            Me.DataColumn1.ColumnName = "OrderID"
            Me.DataColumn1.DataType = GetType(Integer)
            '
            'DataColumn2
            '
            Me.DataColumn2.ColumnName = "CreaterCode"
            '
            'DataColumn3
            '
            Me.DataColumn3.ColumnName = "ItemID"
            '
            'DataColumn4
            '
            Me.DataColumn4.Caption = "OriginalName"
            Me.DataColumn4.ColumnName = "OriginalName"
            '
            'DataColumn5
            '
            Me.DataColumn5.ColumnName = "OriginalCr"
            '
            'DataColumn6
            '
            Me.DataColumn6.ColumnName = "Unit"
            '
            'DataColumn7
            '
            Me.DataColumn7.ColumnName = "Volume"
            '
            'DataColumn8
            '
            Me.DataColumn8.ColumnName = "Quantity"
            '
            'DataColumn9
            '
            Me.DataColumn9.ColumnName = "Note"
            '
            'DataColumn10
            '
            Me.DataColumn10.ColumnName = "Period"
            '
            'DataColumn11
            '
            Me.DataColumn11.ColumnName = "Doc"
            '
            'DataColumn12
            '
            Me.DataColumn12.ColumnName = "Junk"
            Me.DataColumn12.DataType = GetType(Boolean)
            '
            'DataColumn13
            '
            Me.DataColumn13.ColumnName = "UpCost"
            Me.DataColumn13.DataType = GetType(Decimal)
            '
            'DataColumn14
            '
            Me.DataColumn14.ColumnName = "Cost"
            Me.DataColumn14.DataType = GetType(Decimal)
            '
            'DataColumn15
            '
            Me.DataColumn15.ColumnName = "SalerID"
            Me.DataColumn15.DataType = GetType(UInteger)
            '
            'DataColumn16
            '
            Me.DataColumn16.ColumnName = "PriceDate"
            '
            'DataColumn17
            '
            Me.DataColumn17.ColumnName = "PrepCode"
            Me.DataColumn17.DataType = GetType(UInteger)
            '
            'dataColumn18
            '
            Me.dataColumn18.ColumnName = "OrderCode1"
            Me.dataColumn18.DataType = GetType(UInteger)
            '
            'dataColumn19
            '
            Me.dataColumn19.ColumnName = "OrderCode2"
            Me.dataColumn19.DataType = GetType(UInteger)
            CType(Me.MyDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DataTable1, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub

        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            'CODEGEN: This procedure is required by the Web Services Designer
            'Do not modify it using the code editor.
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

#End Region

        '
        <WebMethod()> _
        Public Function GetNameFromCatalog(ByVal Name() As String, ByVal Form() As String, ByVal NewEar As Boolean, ByVal OfferOnly As Boolean, ByVal PriceID() As UInt32, ByVal Limit As Int32, ByVal SelStart As Int32) As DataSet
            FunctionName = "GetNameFromCatalog"
            Dim NameStr As String
            Dim Params(1) As String
            Dim PriceNameStr As String
            Dim Inc As Integer
            Dim AMPCode As Boolean = False
            MyDS.Tables.Remove("Prices")
            Try
                MyCn.ConnectionString = "Database=usersettings;Data Source=" & SQLHost & ";User Id=system;Password=123"
Restart:
                If MyCn.State = ConnectionState.Closed Then MyCn.Open()
                MyTrans = MyCn.BeginTransaction()
                MySelCmd.Transaction = MyTrans

                MySelCmd.CommandText = "SET SQL_BIG_SELECTS=1; "
                MySelCmd.CommandText &= "select distinct catalog.FullCode PrepCode, catalog.Name, catalog.Form" & _
                     " from (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.catalog)" & _
                     " left join farm.formrules on formrules.firmcode=pricesdata.pricecode" & _
                     " left join farm.core0 c on   c.firmcode=if(clientsdata.OldCode=0, pricesdata.pricecode, intersection.costcode) and catalog.fullcode=c.fullcode and to_days(now())-to_days(datecurprice)<maxold" & _
                     " left join farm.core0 ampc on ampc.fullcode=catalog.fullcode and ampc.codefirmcr=c.codefirmcr and ampc.firmcode=1864" & _
                     " where DisabledByClient=0" & _
                     " and Disabledbyfirm=0" & _
                     " and DisabledByAgency=0" & _
                     " and intersection.clientcode=" & GetClientCode().ToString & _
                     " and retclientsset.clientcode=intersection.clientcode" & _
                     " and pricesdata.pricecode=intersection.pricecode" & _
                " and clientsdata.firmcode=pricesdata.firmcode"

                If Not PriceID Is Nothing Then
                    Inc = 0
                    MySelCmd.CommandText &= " and ("
                    For Each PriceNameStr In PriceID
                        If PriceNameStr Is Nothing And Len(PriceNameStr) > 0 Then
                            If Inc > 0 Then MySelCmd.CommandText &= " or "
                            Params = FormatFindStr(PriceNameStr, "PriceCode" & Inc, "pricesdata.pricecode")

                            MySelCmd.Parameters.Add("PriceCode" & Inc, Params(1))
                            MySelCmd.CommandText &= Params(0)

                            Inc += 1
                        End If
                    Next
                    If Inc < 1 Then MySelCmd.CommandText &= "1"
                    MySelCmd.CommandText &= ")"
                End If

                If Not Form Is Nothing Then
                    Inc = 0
                    MySelCmd.CommandText &= " and ("
                    For Each PriceNameStr In Form
                        If Not PriceNameStr Is Nothing And Len(PriceNameStr) > 0 Then
                            If Inc > 0 Then MySelCmd.CommandText &= " or "
                            Params = FormatFindStr(PriceNameStr, "Form" & Inc, "catalog.form")

                            MySelCmd.Parameters.Add("Form" & Inc, Params(1))
                            MySelCmd.CommandText &= Params(0)

                            Inc += 1
                        End If
                        If Inc < 1 Then MySelCmd.CommandText &= "1"
                    Next
                    MySelCmd.CommandText &= ")"
                End If

                MySelCmd.CommandText &= " and clientsdata.firmstatus=1" & _
                     " and clientsdata.billingstatus=1" & _
                     " and clientsdata.firmtype=0"

                If NewEar Then MySelCmd.CommandText &= " and ampc.id is null"
                If OfferOnly Or PriceID.Length > 0 Then MySelCmd.CommandText &= " and c.id is not null"

                MySelCmd.CommandText &= " and clientsdata.firmsegment=AClientsData.firmsegment" & _
               " and pricesregionaldata.regioncode=intersection.regioncode" & _
               " and pricesregionaldata.pricecode=pricesdata.pricecode" & _
               " and AClientsData.firmcode=intersection.clientcode" & _
               " and (clientsdata.maskregion & intersection.regioncode)>0" & _
               " and (AClientsData.maskregion & intersection.regioncode)>0" & _
               " and (retclientsset.workregionmask & intersection.regioncode)>0" & _
               " and pricesdata.agencyenabled=1" & _
               " and pricesdata.enabled=1 and invisibleonclient=0" & _
               " and pricesdata.pricetype<>1" & _
               " and pricesregionaldata.enabled=1"

                If Not Name Is Nothing Then
                    Inc = 0
                    MySelCmd.CommandText &= " and ("
                    If IsNumeric(Name(0)) Then AMPCode = True
                    For Each NameStr In Name

                        If Len(NameStr) > 0 Then


                            If Inc > 0 Then MySelCmd.CommandText &= " or "

                            If AMPCode Then
                                Params = FormatFindStr(NameStr, "Name" & Inc, "ampc.code")
                            Else
                                Params = FormatFindStr(NameStr, "Name" & Inc, "catalog.Name")
                            End If


                            MySelCmd.Parameters.Add("Name" & Inc, Params(1))
                            MySelCmd.CommandText &= Params(0)

                            Inc += 1
                        End If
                    Next
                    MySelCmd.CommandText &= ")"
                End If
                MySelCmd.CommandText &= " order by catalog.Name, catalog.Form"
                If SelStart.ToString.Length > 0 Then
                    MySelCmd.CommandText &= " limit " & SelStart

                    If Limit.ToString.Length > 0 Then
                        MySelCmd.CommandText &= "," & Limit
                    End If

                End If

                MySelCmd.CommandText &= ";"

                LogQuery(MyDA.Fill(MyDS, "Catalog"), FunctionName, StartTime)

                MyTrans.Commit()
                Return MyDS

            Catch MySQLErr As MySqlException
                If Not MyTrans Is Nothing Then MyTrans.Rollback()
                If MySQLErr.Number = 1213 Or MySQLErr.Number = 1205 Then
                    System.Threading.Thread.Sleep(100)
                    GoTo Restart
                End If
                MailErr(FunctionName, MySQLErr.Message, MySQLErr.Source, UserName)

            Catch ex As Exception
                If Not MyTrans Is Nothing Then MyTrans.Rollback()
                MailErr(FunctionName, ex.Message, ex.Source, UserName)
            Finally
                If MyCn.State = ConnectionState.Open Then MyCn.Close()
            End Try


        End Function

        <WebMethod()> _
        Public Function GetPricesByPrepCode(ByVal PrepCode() As Int32, ByVal OnlyLeader As Boolean, _
        ByVal PriceID() As UInt32, ByVal Limit As Int32, ByVal SelStart As Int32) As DataSet
            FunctionName = "GetPricesByPrepCode"
            Dim FullCode, inc As Int32
            Dim PriceNameStr As String
            Dim Params(1) As String



            Dim FullCodesString As String = "("
            Try
                MyCn.ConnectionString = "Database=usersettings;Data Source=" & SQLHost & ";User Id=system;Password=123"
restart:
                If MyCn.State = ConnectionState.Closed Then MyCn.Open()
                MyTrans = MyCn.BeginTransaction(IsolationLevel.ReadCommitted)
                MySelCmd.Transaction = MyTrans

                For Each FullCode In PrepCode
                    If Len(FullCodesString) > 1 And FullCode > 0 Then FullCodesString &= ", "
                    If FullCode > 0 Then FullCodesString &= FullCode
                Next
                FullCodesString &= ")"
                'index FullCode(FullCode), index CodeFirmCr(CodeFirmCr), index Cost(Cost), index junk(junk)
                'index mincost(mincost), index fullcode(FullCode), index CodeFirmcr(CodeFirmCr), index Junk(Junk)

                ' If OnlyLeader Then

                MySelCmd.CommandText = "DROP temporary table IF EXISTS prices; " & _
"DROP temporary table IF EXISTS mincosts; " & _
"create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20) not null default 0, CreaterCode varchar(20) not null default 0, ItemID varchar(50) not null default 0, OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15) not null default 0, Volume varchar(15) not null default 0, Quantity varchar(15) not null default 0, Note varchar(50) not null default 0, Period varchar(20) not null default 0, Doc varchar(20) not null default 0, Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20), PriceDate varchar(20), FullCode int(32) unsigned, CodeFirmCr int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned, primary key ID(OrderID))type= innodb; " & _
"create temporary table mincosts( MinCost decimal(8,2), FullCode int(32) unsigned, Junk Bit) type                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              = innodb; " & _
"INSERT " & _
"INTO prices " & _
"SELECT  c.id OrderID, " & _
"        c.Code, " & _
"        c.CodeCr, " & _
"        null, " & _
"        s.synonym OriginalName, " & _
"        scr.synonym, " & _
"        c.Unit, " & _
"        c.Volume, " & _
"        c.Quantity, " & _
"        c.Note, " & _
"        c.Period, " & _
"        c.Doc, " & _
"        length(c.Junk)< 1 Junk, " & _
"        intersection.PublicCostCorr As UpCost, " & _
"        round(if((1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100) *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost< c.minboundcost, c.minboundcost, (1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100) *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost), 2) Cost, " & _
"        pricesdata.pricecode SalerID, " & _
"        ClientsData.ShortName SalerName, " & _
"        if(fr.datelastform> fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate, " & _
"        c.fullcode, " & _
"        c.codefirmcr, " & _
"        c.synonymcode OrderCode1, " & _
"        c.synonymfirmcrcode OrderCode2 " & _
"FROM    (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.core0 c, farm.synonym s, farm.formrules fr) " & _
"LEFT JOIN farm.synonymfirmcr scr " & _
"        ON scr.firmcode                                             = ifnull(parentsynonym, pricesdata.pricecode) " & _
"        and c.synonymfirmcrcode                                     = scr.synonymfirmcrcode " & _
"WHERE   DisabledByClient                                            = 0 " & _
"        and Disabledbyfirm                                          = 0 " & _
"        and DisabledByAgency                                        = 0 " & _
"        and intersection.clientcode                                 = " & GetClientCode().ToString & _
"        and retclientsset.clientcode                                = intersection.clientcode " & _
"        and pricesdata.pricecode                                    = intersection.pricecode " & _
"        and clientsdata.firmcode                                    = pricesdata.firmcode " & _
"        and clientsdata.firmstatus                                  = 1 " & _
"        and clientsdata.billingstatus                               = 1 " & _
"        and clientsdata.firmtype                                    = 0 " & _
"        and clientsdata.firmsegment                                 = AClientsData.firmsegment " & _
"        and pricesregionaldata.regioncode                           = intersection.regioncode " & _
"        and pricesregionaldata.pricecode                            = pricesdata.pricecode " & _
"        and AClientsData.firmcode                                   = intersection.clientcode " & _
"        and (clientsdata.maskregion & intersection.regioncode)      > 0 " & _
"        and (AClientsData.maskregion & intersection.regioncode)     > 0 " & _
"        and (retclientsset.workregionmask & intersection.regioncode)> 0 " & _
"        and pricesdata.agencyenabled                                = 1 " & _
"        and pricesdata.enabled                                      = 1 " & _
"        and invisibleonclient                                       = 0 " & _
"        and pricesdata.pricetype                                   <> 1 " & _
"        and to_days(now())-to_days(datecurprice)                    < maxold " & _
"        and pricesregionaldata.enabled                              = 1 " & _
"        and fr.firmcode                                             = pricesdata.pricecode " & _
"        and c.firmcode                                              = intersection.costcode " & _
"        and c.fullcode                                              in " & FullCodesString & _
"        and c.synonymcode                                           = s.synonymcode " & _
"        and s.firmcode                     = ifnull(parentsynonym, pricesdata.pricecode) "

                If Not PriceID Is Nothing Then
                    inc = 0
                    MySelCmd.CommandText &= " and ("

                    For Each PriceNameStr In PriceID

                        If inc > 0 Then MySelCmd.CommandText &= " or "
                        Params = FormatFindStr(PriceNameStr, "ShortName" & inc, "pricesdata.pricecode")


                        MySelCmd.Parameters.Add("ShortName" & inc, Params(1))
                        MySelCmd.CommandText &= Params(0)

                        inc += 1
                    Next
                    MySelCmd.CommandText &= ")"
                End If

                MySelCmd.CommandText &= "ORDER BY 1, 15 "

                If SelStart.ToString.Length > 0 Then
                    MySelCmd.CommandText &= " limit " & SelStart

                    If Limit.ToString.Length > 0 Then
                        MySelCmd.CommandText &= "," & Limit
                    End If

                End If
                MySelCmd.CommandText &= "; "

                '                MySelCmd.CommandText &= "UPDATE prices p, " & _
                '"        farm.core0 c " & _
                '"        SET ItemID      = code " & _
                '"WHERE   c.firmcode      = 1864 " & _
                '"        and c.fullcode  = p.fullcode " & _
                '"        and c.codefirmcr= p.codefirmcr ;"


                '                If OnlyLeader Then MySelCmd.CommandText &= ", c.codefirmcr"

               

                MySelCmd.CommandText &= "SELECT  OrderID, " & _
                "        SalerCode, " & _
                "        CreaterCode, " & _
                "        ItemID, " & _
                "        OriginalName, " & _
                "        OriginalCr, " & _
                "        Unit, " & _
                "        Volume, " & _
                "        Quantity, " & _
                "        Note, " & _
                "        Period, " & _
                "        Doc, " & _
                "        Junk, " & _
                "        UpCost, " & _
                "        Cost, " & _
                "        SalerID, " & _
                "        SalerName, " & _
                "        PriceDate, " & _
                "        FullCode PrepCode, " & _
                "        synonymcode OrderCode1, " & _
                "        synonymfirmcrcode OrderCode2 " & _
                "FROM    prices"
              


           

                '            If OnlyLeader Then MySelCmd.CommandText &= " insert into mincosts" & _
                '                                        " select min(cost), FullCode, Junk from (prices)" & _
                '                                        " group by FullCode,  Junk;" & _
                '            " select  OrderID, SalerCode, CreaterCode, ItemID, OriginalName, " & _
                '            " OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, p.Junk, UpCost," & _
                '            " Cost, SalerID, SalerName, PriceDate, p.FullCode PrepCode, synonymcode OrderCode1, synonymfirmcrcode OrderCode2" & _
                '" from (prices p, mincosts m)" & _
                '" where p.fullcode=m.fullcode" & _
                '" and p.cost=m.mincost" & _
                '            " and p.junk=m.junk"

                '            MySelCmd.CommandText &= " order by 1, 15"

                '            If SelStart.ToString.Length > 0 Then
                '                MySelCmd.CommandText &= " limit " & SelStart

                '                If Limit.ToString.Length > 0 Then
                '                    MySelCmd.CommandText &= "," & Limit
                '                End If

                '            End If

                LogQuery(MyDA.Fill(MyDS, "Prices"), FunctionName, StartTime)

                MyTrans.Commit()
                Return MyDS

            Catch MySQLErr As MySqlException
                If Not MyTrans Is Nothing Then MyTrans.Rollback()
                If MySQLErr.Number = 1213 Or MySQLErr.Number = 1205 Then
                    System.Threading.Thread.Sleep(100)
                    GoTo Restart
                End If
                MailErr(FunctionName, MySQLErr.Message, MySQLErr.Source, UserName)

            Catch ex As Exception
                If Not MyTrans Is Nothing Then MyTrans.Rollback()
                MailErr(FunctionName, ex.Message, ex.Source, UserName)
            Finally
                If MyCn.State = ConnectionState.Open Then MyCn.Close()
            End Try

        End Function

        <WebMethod()> _
        Public Function GetPricesByItemID(ByVal ItemID As String(), _
        ByVal OnlyLeader As Boolean, ByVal SalerName() As String, ByVal Limit As Int32, ByVal SelStart As Int32) As DataSet
            Dim NameStr, PriceNameStr, AMPCodes As String
            Dim Params(1) As String
            Dim Inc, AMPCodesArrID, NotAMPCodesArrID, SepIndex As Int32
            Dim AMPCodesArr(-1), NotAMPCodesArr(-1) As Int32
            Dim NotID As Boolean
            FunctionName = "GetPricesByItemID"
            Try
                MyCn.ConnectionString = "Database=usersettings;Data Source=" & SQLHost & ";User Id=system;Password=123"
Restart:
                If MyCn.State = ConnectionState.Closed Then MyCn.Open()
                MyTrans = MyCn.BeginTransaction()
                MySelCmd.Transaction = MyTrans

                MySelCmd.CommandText = " drop temporary table IF EXISTS PrpCodes;" & _
                                        " create temporary table PrpCodes(Fullcode int(32) unsigned, CodeFirmCr int(32) unsigned, Code varchar(20), key FullCode(FullCode), key CodeFirmCr(CodeFirmCr));" & _
                                        " insert into PrpCodes" & _
                                        " select distinct ampc.fullcode, ampc.codefirmcr, ampc.code" & _
                                        " from  (farm.core0 ampc) " & _
                                        " where ampc.firmcode=1864"



                If Not ItemID Is Nothing Then

                    For Each AMPCodes In ItemID
                        If Len(AMPCodes) > 0 Then
                            If AMPCodes.StartsWith("!") Then
                                AMPCodes = AMPCodes.Substring(1)
                                NotID = True
                            Else
                                NotID = False
                            End If

                            If AMPCodes.IndexOf("..") > 0 Then
                                SepIndex = AMPCodes.IndexOf("..")

                                If CInt(Left(AMPCodes, SepIndex)) < CInt(Right(AMPCodes, AMPCodes.Length - SepIndex - 2)) Then
                                    If CInt(Right(AMPCodes, AMPCodes.Length - SepIndex - 2)) - CInt(Left(AMPCodes, SepIndex)) > 20000 Then Err.Raise(1, "Owerflow")
                                    If NotID Then

                                        ReDim Preserve NotAMPCodesArr(NotAMPCodesArr.Length + Right(AMPCodes, AMPCodes.Length - SepIndex - 2) - Left(AMPCodes, SepIndex))

                                    Else

                                        ReDim Preserve AMPCodesArr(AMPCodesArr.Length + Right(AMPCodes, AMPCodes.Length - SepIndex - 2) - Left(AMPCodes, SepIndex))

                                    End If
                                    For Inc = Left(AMPCodes, SepIndex) To Right(AMPCodes, AMPCodes.Length - SepIndex - 2)

                                        If NotID Then
                                            NotAMPCodesArr(NotAMPCodesArrID) = Inc
                                            NotAMPCodesArrID += 1
                                        Else
                                            AMPCodesArr(AMPCodesArrID) = Inc
                                            AMPCodesArrID += 1
                                        End If

                                    Next
                                End If

                            Else
                                If NotID Then
                                    ReDim Preserve NotAMPCodesArr(NotAMPCodesArr.Length)
                                    NotAMPCodesArr(NotAMPCodesArrID) = AMPCodes
                                    NotAMPCodesArrID += 1
                                Else
                                    ReDim Preserve AMPCodesArr(AMPCodesArr.Length)
                                    AMPCodesArr(AMPCodesArrID) = AMPCodes
                                    AMPCodesArrID += 1
                                End If

                            End If
                        End If
                    Next

                End If

                If NotAMPCodesArr.Length > 0 Then
                    MySelCmd.CommandText &= " and ampc.code not in ("
                    For Inc = 0 To NotAMPCodesArr.Length - 1
                        MySelCmd.CommandText &= "'" & NotAMPCodesArr(Inc) & "'"
                        If Inc < NotAMPCodesArr.Length - 1 Then MySelCmd.CommandText &= ","
                    Next
                    MySelCmd.CommandText &= ")"
                End If

                If AMPCodesArr.Length > 0 Then
                    MySelCmd.CommandText &= " and ampc.code in ("
                    For Inc = 0 To AMPCodesArr.Length - 1
                        MySelCmd.CommandText &= AMPCodesArr(Inc)
                        If Inc < AMPCodesArr.Length - 1 Then MySelCmd.CommandText &= ","
                    Next
                    MySelCmd.CommandText &= ")"
                End If

                MySelCmd.CommandText &= ";"

                If OnlyLeader Then MySelCmd.CommandText &= "drop temporary table IF EXISTS prices; drop temporary table IF EXISTS mincosts;" & _
" create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20), CreaterCode varchar(20), ItemID varchar(20)," & _
" OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15), Volume varchar(15), Quantity varchar(15), Note varchar(50)," & _
" Period varchar(20), Doc varchar(20), Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20)," & _
" PriceDate varchar(20), FullCode int(32) unsigned, CodeFirmCr int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned" & _
" );" & _
" create temporary table mincosts( MinCost decimal(8,2), FullCode int(32) unsigned,  Junk Bit" & _
" );" & _
" insert into prices "

                MySelCmd.CommandText &= " select  c.id OrderID, ifnull(c.Code, '') SalerCode, ifnull(c.CodeCr, '') CreaterCode, ifnull(pcd.code, '') ItemID, s.synonym OriginalName, ifnull(scr.synonym, '') OriginalCr, " & _
           " ifnull(c.Unit, '') Unit, ifnull(c.Volume, '') Volume, ifnull(c.Quantity, '') Quantity, ifnull(c.Note, '') Note, ifnull(c.Period, '') Period, ifnull(c.Doc, '') Doc," & _
           " c.Junk>0 Junk," & _
           " intersection.PublicCostCorr As UpCost," & _
           " round(if((1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100)" & _
           " *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost<c.minboundcost, c.minboundcost, (1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100)" & _
           " *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost), 2) Cost," & _
           " pricesdata.pricecode SalerID, ClientsData.ShortName SalerName, if(fr.datelastform>fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate, c.fullcode PrepCode"

                If OnlyLeader Then MySelCmd.CommandText &= ", c.codefirmcr"

                MySelCmd.CommandText &= " , c.synonymcode OrderCode1, c.synonymfirmcrcode OrderCode2 from (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.core0 c," & _
               " farm.synonym s,farm.formrules fr, PrpCodes pcd)" & _
               " left join    farm.synonymfirmcr scr on scr.firmcode=ifnull(parentsynonym, pricesdata.pricecode)  and c.synonymfirmcrcode=scr.synonymfirmcrcode " & _
                " where DisabledByClient=0" & _
                " and Disabledbyfirm=0" & _
                " and DisabledByAgency=0" & _
                " and intersection.clientcode=" & GetClientCode().ToString & _
                " and retclientsset.clientcode=intersection.clientcode" & _
                " and pricesdata.pricecode=intersection.pricecode" & _
                " and clientsdata.firmcode=pricesdata.firmcode" & _
                " and clientsdata.firmstatus=1" & _
                " and clientsdata.billingstatus=1" & _
                " and clientsdata.firmtype=0" & _
                " and to_days(now())-to_days(datecurprice)<maxold"

                MySelCmd.CommandText &= " and clientsdata.firmsegment=AClientsData.firmsegment" & _
               " and pricesregionaldata.regioncode=intersection.regioncode" & _
               " and pricesregionaldata.pricecode=pricesdata.pricecode" & _
               " and AClientsData.firmcode=intersection.clientcode" & _
               " and (clientsdata.maskregion & intersection.regioncode)>0" & _
               " and (AClientsData.maskregion & intersection.regioncode)>0" & _
               " and (retclientsset.workregionmask & intersection.regioncode)>0" & _
               " and pricesdata.agencyenabled=1" & _
               " and pricesdata.enabled=1 and invisibleonclient=0" & _
               " and pricesdata.pricetype<>1" & _
               " and pricesregionaldata.enabled=1" & _
               " and fr.firmcode=pricesdata.pricecode" & _
               " and c.firmcode=if(clientsdata.OldCode=0, pricesdata.pricecode, intersection.costcode)" & _
               " and c.synonymcode=s.synonymcode" & _
               " and s.firmcode=ifnull(parentsynonym, pricesdata.pricecode)"


                If Not SalerName Is Nothing Then
                    Inc = 0
                    MySelCmd.CommandText &= " and ("
                    For Each PriceNameStr In SalerName
                        If Inc > 0 Then MySelCmd.CommandText &= " or "
                        'Params = FormatFindStr(PriceNameStr, "ShortName" & inc, "clientsdata.shortname")
                        'АМП захотели не название поставщика, а название прайс листа.
                        Params = FormatFindStr(PriceNameStr, "ShortName" & Inc, "pricesdata.pricename")

                        MySelCmd.Parameters.Add("ShortName" & Inc, Params(1))
                        MySelCmd.CommandText &= Params(0)

                        Inc += 1
                    Next
                    MySelCmd.CommandText &= ")"
                End If


                MySelCmd.CommandText &= "and pcd.fullcode=c.fullcode and pcd.codefirmcr=c.codefirmcr group by 1;"

                If OnlyLeader Then MySelCmd.CommandText &= " insert into mincosts" & _
                                           " select min(cost), FullCode, Junk from (prices)" & _
                                           " group by FullCode, Junk;" & _
                " select  OrderID, SalerCode, CreaterCode, ItemID, OriginalName, " & _
                " OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, p.Junk, UpCost," & _
                " Cost, SalerID, SalerName, PriceDate, p.FullCode PrepCode, synonymcode OrderCode1, synonymfirmcrcode OrderCode2" & _
    " from (prices p, mincosts m)" & _
    " where p.fullcode=m.fullcode" & _
    " and p.cost=m.mincost" & _
                " and p.junk=m.junk"

                MySelCmd.CommandText &= " order by 1, 15"

                If SelStart.ToString.Length > 0 Then
                    MySelCmd.CommandText &= " limit " & SelStart

                    If Limit.ToString.Length > 0 Then
                        MySelCmd.CommandText &= "," & Limit
                    End If

                End If

                LogQuery(MyDA.Fill(MyDS, "Prices"), FunctionName, StartTime)

                MyTrans.Commit()
                Return MyDS

            Catch MySQLErr As MySqlException
                If Not MyTrans Is Nothing Then MyTrans.Rollback()
                If MySQLErr.Number = 1213 Or MySQLErr.Number = 1205 Then
                    System.Threading.Thread.Sleep(100)
                    GoTo Restart
                End If
                MailErr(FunctionName, MySQLErr.Message, MySQLErr.Source, UserName)

            Catch ex As Exception
                If Not MyTrans Is Nothing Then MyTrans.Rollback()
                MailErr(FunctionName, ex.Message, ex.Source, UserName)
            Finally
                If MyCn.State = ConnectionState.Open Then MyCn.Close()
            End Try
        End Function

        <WebMethod()> _
      Public Function GetPricesByName(ByVal OriginalName() As String, ByVal SalerName() As String, _
      ByVal PriceName() As String, ByVal OnlyLeader As Boolean, ByVal NewEar As Boolean, ByVal Limit As Int32, ByVal SelStart As Int32) As DataSet
            Dim NameStr, PriceNameStr As String
            Dim Params(1) As String
            Dim Inc As Integer
            FunctionName = "GetPricesByName"
            Try
                MyCn.ConnectionString = "Database=usersettings;Data Source=" & SQLHost & ";User Id=system;Password=123"
Restart:
                If MyCn.State = ConnectionState.Closed Then MyCn.Open()
                MyTrans = MyCn.BeginTransaction()
                MySelCmd.Transaction = MyTrans

                If OnlyLeader Then MySelCmd.CommandText = "drop temporary table IF EXISTS prices; drop temporary table IF EXISTS mincosts;" & _
" create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20), CreaterCode varchar(20), ItemID varchar(20)," & _
" OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15), Volume varchar(15), Quantity varchar(15), Note varchar(50)," & _
" Period varchar(20), Doc varchar(20), Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20)," & _
" PriceDate varchar(20), FullCode int(32) unsigned, CodeFirmCr int(32) unsigned, SynonymCode int(32) unsigned, SynonymFirmCrCode int(32) unsigned" & _
" );" & _
" create temporary table mincosts( MinCost decimal(8,2), FullCode int(32) unsigned,  Junk Bit" & _
" );" & _
" insert into prices "

                MySelCmd.CommandText &= " select  c.id OrderID, ifnull(c.Code, '') SalerCode, ifnull(c.CodeCr, '') CreaterCode, ifnull(ampc.code, '') ItemID, s.synonym OriginalName, ifnull(scr.synonym, '') OriginalCr, " & _
           " ifnull(c.Unit, '') Unit, ifnull(c.Volume, '') Volume, ifnull(c.Quantity, '') Quantity, ifnull(c.Note, '') Note, ifnull(c.Period, '') Period, ifnull(c.Doc, '') Doc," & _
           " c.Junk>0 Junk," & _
           " intersection.PublicCostCorr As UpCost," & _
           " round(if((1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100)" & _
           " *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost<c.minboundcost, c.minboundcost, (1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100)" & _
           " *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost), 2) Cost," & _
           " pricesdata.pricecode SalerID, ClientsData.ShortName SalerName, if(fr.datelastform>fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate, c.fullcode PrepCode"

                If OnlyLeader Then MySelCmd.CommandText &= ", c.codefirmcr"

                MySelCmd.CommandText &= " , c.synonymcode OrderCode1, c.synonymfirmcrcode OrderCode2 from (intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.core0 c," & _
               " farm.synonym s, farm.formrules fr)" & _
                " left join farm.core0 ampc on ampc.fullcode=c.fullcode and ampc.codefirmcr=c.codefirmcr and ampc.firmcode=1864" & _
                " left join    farm.synonymfirmcr scr on scr.firmcode=ifnull(parentsynonym, pricesdata.pricecode)  and c.synonymfirmcrcode=scr.synonymfirmcrcode " & _
                " where DisabledByClient=0" & _
                " and Disabledbyfirm=0" & _
                " and DisabledByAgency=0" & _
                " and intersection.clientcode=" & GetClientCode().ToString & _
                " and retclientsset.clientcode=intersection.clientcode" & _
                " and pricesdata.pricecode=intersection.pricecode" & _
                " and clientsdata.firmcode=pricesdata.firmcode" & _
                " and clientsdata.firmstatus=1" & _
                " and clientsdata.billingstatus=1" & _
                " and clientsdata.firmtype=0" & _
                 " and to_days(now())-to_days(datecurprice)<maxold"


                If NewEar Then MySelCmd.CommandText &= " and ampc.id is null"

                MySelCmd.CommandText &= " and clientsdata.firmsegment=AClientsData.firmsegment" & _
               " and pricesregionaldata.regioncode=intersection.regioncode" & _
               " and pricesregionaldata.pricecode=pricesdata.pricecode" & _
               " and AClientsData.firmcode=intersection.clientcode" & _
               " and (clientsdata.maskregion & intersection.regioncode)>0" & _
               " and (AClientsData.maskregion & intersection.regioncode)>0" & _
               " and (retclientsset.workregionmask & intersection.regioncode)>0" & _
               " and pricesdata.agencyenabled=1" & _
               " and pricesdata.enabled=1 and invisibleonclient=0" & _
               " and pricesdata.pricetype<>1" & _
               " and pricesregionaldata.enabled=1" & _
               " and fr.firmcode=pricesdata.pricecode" & _
               " and c.firmcode=if(clientsdata.OldCode=0, pricesdata.pricecode, intersection.costcode)" & _
               " and c.synonymcode=s.synonymcode" & _
               " and s.firmcode=ifnull(parentsynonym, pricesdata.pricecode)"


                If Not SalerName Is Nothing Then
                    Inc = 0
                    MySelCmd.CommandText &= " and ("
                    For Each PriceNameStr In SalerName
                        If Inc > 0 Then MySelCmd.CommandText &= " or "
                        Params = FormatFindStr(PriceNameStr, "ShortName" & Inc, "clientsdata.shortname")
                        'АМП захотели не название поставщика, а название прайс листа.
                        'Params = FormatFindStr(PriceNameStr, "ShortName" & Inc, "pricesdata.pricename")

                        MySelCmd.Parameters.Add("ShortName" & Inc, Params(1))
                        MySelCmd.CommandText &= Params(0)

                        Inc += 1
                    Next
                    MySelCmd.CommandText &= ")"
                End If

                If Not PriceName Is Nothing Then
                    Inc = 0
                    MySelCmd.CommandText &= " and ("
                    For Each PriceNameStr In SalerName
                        If Inc > 0 Then MySelCmd.CommandText &= " or "
                        'Params = FormatFindStr(PriceNameStr, "ShortName" & inc, "clientsdata.shortname")
                        'АМП захотели не название поставщика, а название прайс листа.
                        Params = FormatFindStr(PriceNameStr, "PriceName" & Inc, "pricesdata.pricename")

                        MySelCmd.Parameters.Add("PriceName" & Inc, Params(1))
                        MySelCmd.CommandText &= Params(0)

                        Inc += 1
                    Next
                    MySelCmd.CommandText &= ")"
                End If

                If Not OriginalName Is Nothing Then
                    Inc = 0
                    MySelCmd.CommandText &= " and ("
                    For Each NameStr In OriginalName
                        If Len(NameStr) > 0 Then

                            If Inc > 0 Then MySelCmd.CommandText &= " or "
                            Params = FormatFindStr(NameStr, "Name" & Inc, "s.synonym")

                            MySelCmd.Parameters.Add("Name" & Inc, Params(1))
                            MySelCmd.CommandText &= Params(0)

                            Inc += 1
                        End If
                    Next
                    MySelCmd.CommandText &= ")"
                End If
                MySelCmd.CommandText &= " group by 1;"

                If OnlyLeader Then MySelCmd.CommandText &= " insert into mincosts" & _
                                           " select min(cost), FullCode, Junk from (prices)" & _
                                           " group by FullCode,  Junk;" & _
               " select  OrderID, SalerCode, CreaterCode, ItemID, OriginalName, OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, p.Junk, UpCost, Cost, SalerID, SalerName,  PriceDate, p.FullCode PrepCode, synonymcode OrderCode1, synonymfirmcrcode OrderCode2" & _
    " from (prices p, mincosts m)" & _
    " where p.fullcode=m.fullcode" & _
    " and p.cost=m.mincost" & _
               " and p.junk=m.junk"

                MySelCmd.CommandText &= " order by 1, 15"

                If SelStart.ToString.Length > 0 Then
                    MySelCmd.CommandText &= " limit " & SelStart

                    If Limit.ToString.Length > 0 Then
                        MySelCmd.CommandText &= "," & Limit
                    End If

                End If

                LogQuery(MyDA.Fill(MyDS, "Prices"), FunctionName, StartTime)

                MyTrans.Commit()
                Return MyDS

            Catch MySQLErr As MySqlException
                If Not MyTrans Is Nothing Then MyTrans.Rollback()
                If MySQLErr.Number = 1213 Or MySQLErr.Number = 1205 Then
                    System.Threading.Thread.Sleep(100)
                    GoTo Restart
                End If
                MailErr(FunctionName, MySQLErr.Message, MySQLErr.Source, UserName)

            Catch ex As Exception
                If Not MyTrans Is Nothing Then MyTrans.Rollback()
                MailErr(FunctionName, ex.Message, ex.Source, UserName)
            Finally
                If MyCn.State = ConnectionState.Open Then MyCn.Close()
            End Try

        End Function

        '<WebMethod()> _
        '    Public Function GetPricesBySalerName(ByVal SalerName() As String, ByVal OnlyLeader As Boolean, ByVal NewEar As Boolean) As DataSet

        '        Dim PriceNameStr As String
        '        Dim Params(1) As String
        '        Dim Inc As Integer

        '        Try
        '            MyCn.ConnectionString = "Database=usersettings;Data Source=testsql.analit.net;User Id=system;Password=123"
        'Restart:
        '            If MyCn.State = ConnectionState.Closed Then MyCn.Open()

        '            MyTrans = MyCn.BeginTransaction(IsolationLevel.ReadCommitted)
        '            MySelCmd.Transaction = MyTrans

        '            If OnlyLeader Then MySelCmd.CommandText = "drop temporary table IF EXISTS prices; drop temporary table IF EXISTS mincosts;" & _
        '                  " create temporary table prices(OrderID int(32) unsigned, SalerCode varchar(20), CreaterCode varchar(20), ItemID varchar(20)," & _
        '                    " OriginalName varchar(255), OriginalCr varchar(255), Unit varchar(15), Volume varchar(15), Quantity varchar(15), Note varchar(50)," & _
        '                    " Period varchar(20), Doc varchar(20), Junk Bit, UpCost decimal(5,3), Cost Decimal(8,2), SalerID int(32) unsigned, SalerName varchar(20), PriceDate varchar(20), FullCode int(32) unsigned, CodeFirmCr int(32) unsigned" & _
        '                    " ) type=InnoDB;" & _
        '                    " create temporary table mincosts( MinCost decimal(8,2), FullCode int(32) unsigned, CodeFirmCr int(32) unsigned, Junk Bit" & _
        '                    " )type=InnoDB;" & _
        '                    " insert into prices "

        '            MySelCmd.CommandText &= " select  c.id OrderID, ifnull(c.Code, '') SalerCode, ifnull(c.CodeCr, '') CreaterCode, ifnull(ampc.code, '') ItemID, s.synonym OriginalName, scr.synonym OriginalCr, " & _
        '           " ifnull(c.Unit, '') Unit, ifnull(c.Volume, '') Volume, ifnull(c.Quantity, '') Quantity, ifnull(c.Note, '') Note, ifnull(c.Period, '') Period, ifnull(c.Doc, '') Doc," & _
        '           " c.Junk>0 Junk," & _
        '           " intersection.PublicCostCorr As UpCost," & _
        '           " round(if((1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100)" & _
        '           " *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost<c.minboundcost, c.minboundcost, (1+pricesdata.UpCost/100)*(1+pricesregionaldata.UpCost/100)" & _
        '           " *(1+(intersection.PublicCostCorr+intersection.FirmCostCorr)/100) *c.BaseCost), 2) Cost," & _
        '           " pricesdata.pricecode SalerID, ClientsData.ShortName SalerName, if(fr.datelastform>fr.DateCurPrice, fr.DateCurPrice, fr.DatePrevPrice) PriceDate" '

        '            If OnlyLeader Then MySelCmd.CommandText &= ", c.fullcode, c.codefirmcr"

        '            MySelCmd.CommandText &= " from intersection, clientsdata, pricesdata, pricesregionaldata, retclientsset, clientsdata as AClientsData, farm.core0 c," & _
        '           " farm.synonym s, farm.synonymfirmcr scr, farm.formrules fr" & _
        '            " left join farm.core0 ampc on ampc.fullcode=c.fullcode and ampc.codefirmcr=c.codefirmcr and ampc.firmcode=1864" & _
        '            " where DisabledByClient=0" & _
        '            " and Disabledbyfirm=0" & _
        '            " and DisabledByAgency=0" & _
        '            " and intersection.clientcode=" & GetClientCode().ToString & _
        '            " and retclientsset.clientcode=intersection.clientcode" & _
        '            " and pricesdata.pricecode=intersection.pricecode" & _
        '            " and clientsdata.firmcode=pricesdata.firmcode" & _
        '            " and clientsdata.firmstatus=1" & _
        '            " and clientsdata.billingstatus=1" & _
        '            " and clientsdata.firmtype=0"

        '            If NewEar Then MySelCmd.CommandText &= " and ampc.id is null"

        '            MySelCmd.CommandText &= " and clientsdata.firmsegment=AClientsData.firmsegment" & _
        '           " and pricesregionaldata.regioncode=intersection.regioncode" & _
        '           " and pricesregionaldata.pricecode=pricesdata.pricecode" & _
        '           " and AClientsData.firmcode=intersection.clientcode" & _
        '           " and (clientsdata.maskregion & intersection.regioncode)>0" & _
        '           " and (AClientsData.maskregion & intersection.regioncode)>0" & _
        '           " and (retclientsset.workregionmask & intersection.regioncode)>0" & _
        '           " and pricesdata.agencyenabled=1" & _
        '           " and pricesdata.enabled=1 and invisibleonclient=0" & _
        '           " and pricesdata.pricetype<>1" & _
        '           " and pricesregionaldata.enabled=1" & _
        '           " and fr.firmcode=pricesdata.pricecode" & _
        '           " and c.firmcode=if(clientsdata.OldCode=0, pricesdata.pricecode, intersection.costcode)" & _
        '           " and c.synonymcode=s.synonymcode" & _
        '              " and s.firmcode=ifnull(parentsynonym, pricesdata.pricecode)" & _
        '" and scr.firmcode=ifnull(parentsynonym, pricesdata.pricecode)" & _
        '           " and c.synonymfirmcrcode=scr.synonymfirmcrcode"

        '            Inc = 0

        '            MySelCmd.CommandText &= " and ("
        '            For Each PriceNameStr In SalerName
        '                If Inc > 0 Then MySelCmd.CommandText &= " or "
        '                Params = FormatFindStr(PriceNameStr, "ShortName" & Inc, "clientsdata.shortname")

        '                MySelCmd.Parameters.Add("ShortName" & Inc, Params(1))
        '                MySelCmd.CommandText &= Params(0)

        '                Inc += 1
        '            Next
        '            MySelCmd.CommandText &= ")"


        '            MySelCmd.CommandText &= ";"

        '            If OnlyLeader Then MySelCmd.CommandText &= " insert into mincosts" & _
        '                                       " select min(cost), FullCode, CodeFirmCr, Junk from prices" & _
        '                                       " group by FullCode, CodeFirmCr, Junk;" & _
        '           " select  OrderID, SalerCode, CreaterCode, ItemID, OriginalName, OriginalCr, Unit, Volume, Quantity, Note, Period, Doc, p.Junk, UpCost, Cost, SalerID, SalerName, PriceDate" & _
        '" from prices p, mincosts m" & _
        '" where p.fullcode=m.fullcode" & _
        '" and p.codefirmcr=m.codefirmcr" & _
        '" and p.cost=m.mincost" & _
        '           " and p.junk=m.junk"



        '            MyDA.Fill(MyDS, "Prices")
        '            Return MyDS


        '        Catch MySQLErr As MySqlException
        '            MyTrans.Rollback()
        '            If MySQLErr.Number = 1213 Or MySQLErr.Number = 1205 Then
        '                System.Threading.Thread.Sleep(100)
        '                GoTo Restart
        '            End If
        '        Catch ex As Exception
        '            MyTrans.Rollback()
        '        Finally
        '            If MyCn.State = ConnectionState.Open Then MyCn.Close()
        '        End Try

        '    End Function

        <WebMethod()> _
        Public Function PostOrder(ByVal OrderID() As Int32, ByVal Quantity() As Int32, ByVal Message() As String, _
         ByVal OrderCode1() As Int32, ByVal OrderCode2() As Int32, ByVal Junk() As Boolean) As DataSet
            FunctionName = "PostOrder"
            Try
                MyCn.ConnectionString = "Database=usersettings;Data Source=" & SQLHost & ";User Id=system;Password=123"
Restart:
                If MyCn.State = ConnectionState.Closed Then MyCn.Open()

            Catch err As Exception
                MailErr(FunctionName, err.Message, err.Source, UserName)
            End Try

            Try
                Return Global.AMPService.PostOrder.PostOrderMethod(OrderID, Quantity, Message, _
                 OrderCode1, OrderCode2, Junk, GetClientCode, UserName, SQLHost)
                ' Return MyDS
            Catch err As Exception
                MailErr(FunctionName, err.Message, err.Source, UserName)
            Finally
                If MyCn.State = ConnectionState.Open Then MyCn.Close()

            End Try


        End Function

        Private Function FormatFindStr(ByVal InpStr As String, ByVal ParameterName As String, ByVal FieldName As String) As String()
            Dim UseLike As Boolean
            Dim TmpRes As String = FieldName
            Dim Result(1) As String

            'Если будем использовать LIKE
            If InpStr.IndexOf("*") >= 0 Then UseLike = True


            'Если есть отрицание
            If InpStr.IndexOf("!") >= 0 Then

                InpStr = InpStr.Remove(0, 1)

                If UseLike Then

                    TmpRes &= " not like "

                Else

                    TmpRes &= " <> "

                End If

            Else
                If UseLike Then

                    TmpRes &= " like "

                Else

                    TmpRes &= " = "

                End If
            End If

            InpStr = InpStr.Replace("*", "%")
            TmpRes &= "?" & ParameterName

            Result(0) = New String(TmpRes)
            Result(1) = New String(InpStr)

            Return Result
        End Function

        Private Function GetClientCode() As UInt32
            UserName = HttpContext.Current.User.Identity.Name
            If Left(UserName, 7) = "ANALIT\" Then
                UserName = Mid(UserName, 8)
            End If
            UserName = "amp"
            Try
                MySelCmd.CommandText = " SELECT osuseraccessright.clientcode" & _
                            " FROM (clientsdata, osuseraccessright)" & _
                            " where osuseraccessright.clientcode=clientsdata.firmcode" & _
                            " and firmstatus=1" & _
                            " and billingstatus=1" & _
                            " and allowGetData=1" & _
                            " and OSUserName='" & UserName & "'"

                Return MySelCmd.ExecuteScalar

            Catch ErrorTXT As Exception
                MailErr("GetClientCode", ErrorTXT.Message, ErrorTXT.Source, UserName)
            Finally

            End Try
        End Function

        Private Sub MailErr(ByVal ProcessName As String, ByVal ErrMessage As String, ByVal ErrSource As String, ByVal UserName As String)
            Dim Message As New MailMessage
            Dim Address As New MailAddress("service@analit.net")
            Dim Client As New SmtpClient("box.analit.net")
            Try


                Message.From = Address
                Message.Subject = "Ошибка в AMPService"
                'Message.BodyFormat
                Message.Body = "Процесс: " & ProcessName & Chr(10) & Chr(13) & _
                "Ошибка: " & ErrMessage & Chr(10) & Chr(13) & _
                "Источник: " & ErrSource & Chr(10) & Chr(13) & _
                "Логин: " & UserName
                Message.To.Add(Address)
                Message.BodyEncoding = System.Text.Encoding.UTF8
                Client.Send(Message)

            Catch
            End Try

        End Sub

        Private Function LogQuery(ByVal RowCount As Int32, ByVal FunctionName As String, ByVal StartTime As Date) As Boolean
            MySelCmd.CommandText = " insert into logs.AMPLogs(LogTime, Host, User, Function, RowCount, ProcessingTime) " & _
                                    " values(now(), '" & HttpContext.Current.Request.UserHostAddress & "', '" & _
                                    UserName & "', '" & FunctionName & "', " & RowCount & ", " & CInt(Now.Subtract(StartTime).TotalMilliseconds).ToString & ")"

            MySelCmd.ExecuteNonQuery()
        End Function

    End Class

End Namespace
