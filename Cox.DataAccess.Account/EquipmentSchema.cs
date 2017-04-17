﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace Cox.DataAccess.Account {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class EquipmentSchema : DataSet {
        
        private ActivePpvEventsDataTable tableActivePpvEvents;
        
        private CustomerEquipmentDataTable tableCustomerEquipment;
        
        public EquipmentSchema() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected EquipmentSchema(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["ActivePpvEvents"] != null)) {
                    this.Tables.Add(new ActivePpvEventsDataTable(ds.Tables["ActivePpvEvents"]));
                }
                if ((ds.Tables["CustomerEquipment"] != null)) {
                    this.Tables.Add(new CustomerEquipmentDataTable(ds.Tables["CustomerEquipment"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public ActivePpvEventsDataTable ActivePpvEvents {
            get {
                return this.tableActivePpvEvents;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public CustomerEquipmentDataTable CustomerEquipment {
            get {
                return this.tableCustomerEquipment;
            }
        }
        
        public override DataSet Clone() {
            EquipmentSchema cln = ((EquipmentSchema)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["ActivePpvEvents"] != null)) {
                this.Tables.Add(new ActivePpvEventsDataTable(ds.Tables["ActivePpvEvents"]));
            }
            if ((ds.Tables["CustomerEquipment"] != null)) {
                this.Tables.Add(new CustomerEquipmentDataTable(ds.Tables["CustomerEquipment"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        internal void InitVars() {
            this.tableActivePpvEvents = ((ActivePpvEventsDataTable)(this.Tables["ActivePpvEvents"]));
            if ((this.tableActivePpvEvents != null)) {
                this.tableActivePpvEvents.InitVars();
            }
            this.tableCustomerEquipment = ((CustomerEquipmentDataTable)(this.Tables["CustomerEquipment"]));
            if ((this.tableCustomerEquipment != null)) {
                this.tableCustomerEquipment.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "EquipmentSchema";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/EquipmentSchema.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableActivePpvEvents = new ActivePpvEventsDataTable();
            this.Tables.Add(this.tableActivePpvEvents);
            this.tableCustomerEquipment = new CustomerEquipmentDataTable();
            this.Tables.Add(this.tableCustomerEquipment);
        }
        
        private bool ShouldSerializeActivePpvEvents() {
            return false;
        }
        
        private bool ShouldSerializeCustomerEquipment() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void ActivePpvEventsRowChangeEventHandler(object sender, ActivePpvEventsRowChangeEvent e);
        
        public delegate void CustomerEquipmentRowChangeEventHandler(object sender, CustomerEquipmentRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ActivePpvEventsDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnSERIAL_NUMBER;
            
            private DataColumn columnEVENT_TITLE;
            
            private DataColumn columnEVENT_START_DATE;
            
            private DataColumn columnEVENT_START_TIME;
            
            private DataColumn columnEND_DATE;
            
            private DataColumn columnEND_TIME;
            
            internal ActivePpvEventsDataTable() : 
                    base("ActivePpvEvents") {
                this.InitClass();
            }
            
            internal ActivePpvEventsDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn SERIAL_NUMBERColumn {
                get {
                    return this.columnSERIAL_NUMBER;
                }
            }
            
            internal DataColumn EVENT_TITLEColumn {
                get {
                    return this.columnEVENT_TITLE;
                }
            }
            
            internal DataColumn EVENT_START_DATEColumn {
                get {
                    return this.columnEVENT_START_DATE;
                }
            }
            
            internal DataColumn EVENT_START_TIMEColumn {
                get {
                    return this.columnEVENT_START_TIME;
                }
            }
            
            internal DataColumn END_DATEColumn {
                get {
                    return this.columnEND_DATE;
                }
            }
            
            internal DataColumn END_TIMEColumn {
                get {
                    return this.columnEND_TIME;
                }
            }
            
            public ActivePpvEventsRow this[int index] {
                get {
                    return ((ActivePpvEventsRow)(this.Rows[index]));
                }
            }
            
            public event ActivePpvEventsRowChangeEventHandler ActivePpvEventsRowChanged;
            
            public event ActivePpvEventsRowChangeEventHandler ActivePpvEventsRowChanging;
            
            public event ActivePpvEventsRowChangeEventHandler ActivePpvEventsRowDeleted;
            
            public event ActivePpvEventsRowChangeEventHandler ActivePpvEventsRowDeleting;
            
            public void AddActivePpvEventsRow(ActivePpvEventsRow row) {
                this.Rows.Add(row);
            }
            
            public ActivePpvEventsRow AddActivePpvEventsRow(string SERIAL_NUMBER, string EVENT_TITLE, System.Decimal EVENT_START_DATE, System.Decimal EVENT_START_TIME, System.Decimal END_DATE, System.Decimal END_TIME) {
                ActivePpvEventsRow rowActivePpvEventsRow = ((ActivePpvEventsRow)(this.NewRow()));
                rowActivePpvEventsRow.ItemArray = new object[] {
                        SERIAL_NUMBER,
                        EVENT_TITLE,
                        EVENT_START_DATE,
                        EVENT_START_TIME,
                        END_DATE,
                        END_TIME};
                this.Rows.Add(rowActivePpvEventsRow);
                return rowActivePpvEventsRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                ActivePpvEventsDataTable cln = ((ActivePpvEventsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new ActivePpvEventsDataTable();
            }
            
            internal void InitVars() {
                this.columnSERIAL_NUMBER = this.Columns["SERIAL_NUMBER"];
                this.columnEVENT_TITLE = this.Columns["EVENT_TITLE"];
                this.columnEVENT_START_DATE = this.Columns["EVENT_START_DATE"];
                this.columnEVENT_START_TIME = this.Columns["EVENT_START_TIME"];
                this.columnEND_DATE = this.Columns["END_DATE"];
                this.columnEND_TIME = this.Columns["END_TIME"];
            }
            
            private void InitClass() {
                this.columnSERIAL_NUMBER = new DataColumn("SERIAL_NUMBER", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnSERIAL_NUMBER);
                this.columnEVENT_TITLE = new DataColumn("EVENT_TITLE", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnEVENT_TITLE);
                this.columnEVENT_START_DATE = new DataColumn("EVENT_START_DATE", typeof(System.Decimal), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnEVENT_START_DATE);
                this.columnEVENT_START_TIME = new DataColumn("EVENT_START_TIME", typeof(System.Decimal), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnEVENT_START_TIME);
                this.columnEND_DATE = new DataColumn("END_DATE", typeof(System.Decimal), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnEND_DATE);
                this.columnEND_TIME = new DataColumn("END_TIME", typeof(System.Decimal), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnEND_TIME);
            }
            
            public ActivePpvEventsRow NewActivePpvEventsRow() {
                return ((ActivePpvEventsRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new ActivePpvEventsRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(ActivePpvEventsRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.ActivePpvEventsRowChanged != null)) {
                    this.ActivePpvEventsRowChanged(this, new ActivePpvEventsRowChangeEvent(((ActivePpvEventsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.ActivePpvEventsRowChanging != null)) {
                    this.ActivePpvEventsRowChanging(this, new ActivePpvEventsRowChangeEvent(((ActivePpvEventsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.ActivePpvEventsRowDeleted != null)) {
                    this.ActivePpvEventsRowDeleted(this, new ActivePpvEventsRowChangeEvent(((ActivePpvEventsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.ActivePpvEventsRowDeleting != null)) {
                    this.ActivePpvEventsRowDeleting(this, new ActivePpvEventsRowChangeEvent(((ActivePpvEventsRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveActivePpvEventsRow(ActivePpvEventsRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ActivePpvEventsRow : DataRow {
            
            private ActivePpvEventsDataTable tableActivePpvEvents;
            
            internal ActivePpvEventsRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableActivePpvEvents = ((ActivePpvEventsDataTable)(this.Table));
            }
            
            public string SERIAL_NUMBER {
                get {
                    try {
                        return ((string)(this[this.tableActivePpvEvents.SERIAL_NUMBERColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableActivePpvEvents.SERIAL_NUMBERColumn] = value;
                }
            }
            
            public string EVENT_TITLE {
                get {
                    try {
                        return ((string)(this[this.tableActivePpvEvents.EVENT_TITLEColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableActivePpvEvents.EVENT_TITLEColumn] = value;
                }
            }
            
            public System.Decimal EVENT_START_DATE {
                get {
                    try {
                        return ((System.Decimal)(this[this.tableActivePpvEvents.EVENT_START_DATEColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableActivePpvEvents.EVENT_START_DATEColumn] = value;
                }
            }
            
            public System.Decimal EVENT_START_TIME {
                get {
                    try {
                        return ((System.Decimal)(this[this.tableActivePpvEvents.EVENT_START_TIMEColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableActivePpvEvents.EVENT_START_TIMEColumn] = value;
                }
            }
            
            public System.Decimal END_DATE {
                get {
                    try {
                        return ((System.Decimal)(this[this.tableActivePpvEvents.END_DATEColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableActivePpvEvents.END_DATEColumn] = value;
                }
            }
            
            public System.Decimal END_TIME {
                get {
                    try {
                        return ((System.Decimal)(this[this.tableActivePpvEvents.END_TIMEColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableActivePpvEvents.END_TIMEColumn] = value;
                }
            }
            
            public bool IsSERIAL_NUMBERNull() {
                return this.IsNull(this.tableActivePpvEvents.SERIAL_NUMBERColumn);
            }
            
            public void SetSERIAL_NUMBERNull() {
                this[this.tableActivePpvEvents.SERIAL_NUMBERColumn] = System.Convert.DBNull;
            }
            
            public bool IsEVENT_TITLENull() {
                return this.IsNull(this.tableActivePpvEvents.EVENT_TITLEColumn);
            }
            
            public void SetEVENT_TITLENull() {
                this[this.tableActivePpvEvents.EVENT_TITLEColumn] = System.Convert.DBNull;
            }
            
            public bool IsEVENT_START_DATENull() {
                return this.IsNull(this.tableActivePpvEvents.EVENT_START_DATEColumn);
            }
            
            public void SetEVENT_START_DATENull() {
                this[this.tableActivePpvEvents.EVENT_START_DATEColumn] = System.Convert.DBNull;
            }
            
            public bool IsEVENT_START_TIMENull() {
                return this.IsNull(this.tableActivePpvEvents.EVENT_START_TIMEColumn);
            }
            
            public void SetEVENT_START_TIMENull() {
                this[this.tableActivePpvEvents.EVENT_START_TIMEColumn] = System.Convert.DBNull;
            }
            
            public bool IsEND_DATENull() {
                return this.IsNull(this.tableActivePpvEvents.END_DATEColumn);
            }
            
            public void SetEND_DATENull() {
                this[this.tableActivePpvEvents.END_DATEColumn] = System.Convert.DBNull;
            }
            
            public bool IsEND_TIMENull() {
                return this.IsNull(this.tableActivePpvEvents.END_TIMEColumn);
            }
            
            public void SetEND_TIMENull() {
                this[this.tableActivePpvEvents.END_TIMEColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ActivePpvEventsRowChangeEvent : EventArgs {
            
            private ActivePpvEventsRow eventRow;
            
            private DataRowAction eventAction;
            
            public ActivePpvEventsRowChangeEvent(ActivePpvEventsRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public ActivePpvEventsRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CustomerEquipmentDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnSERIAL_NUMBER;
            
            private DataColumn columnITEM_NUMBER;
            
            private DataColumn columnPORT_TYPE;
            
            internal CustomerEquipmentDataTable() : 
                    base("CustomerEquipment") {
                this.InitClass();
            }
            
            internal CustomerEquipmentDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn SERIAL_NUMBERColumn {
                get {
                    return this.columnSERIAL_NUMBER;
                }
            }
            
            internal DataColumn ITEM_NUMBERColumn {
                get {
                    return this.columnITEM_NUMBER;
                }
            }
            
            internal DataColumn PORT_TYPEColumn {
                get {
                    return this.columnPORT_TYPE;
                }
            }
            
            public CustomerEquipmentRow this[int index] {
                get {
                    return ((CustomerEquipmentRow)(this.Rows[index]));
                }
            }
            
            public event CustomerEquipmentRowChangeEventHandler CustomerEquipmentRowChanged;
            
            public event CustomerEquipmentRowChangeEventHandler CustomerEquipmentRowChanging;
            
            public event CustomerEquipmentRowChangeEventHandler CustomerEquipmentRowDeleted;
            
            public event CustomerEquipmentRowChangeEventHandler CustomerEquipmentRowDeleting;
            
            public void AddCustomerEquipmentRow(CustomerEquipmentRow row) {
                this.Rows.Add(row);
            }
            
            public CustomerEquipmentRow AddCustomerEquipmentRow(string SERIAL_NUMBER, string ITEM_NUMBER, string PORT_TYPE) {
                CustomerEquipmentRow rowCustomerEquipmentRow = ((CustomerEquipmentRow)(this.NewRow()));
                rowCustomerEquipmentRow.ItemArray = new object[] {
                        SERIAL_NUMBER,
                        ITEM_NUMBER,
                        PORT_TYPE};
                this.Rows.Add(rowCustomerEquipmentRow);
                return rowCustomerEquipmentRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                CustomerEquipmentDataTable cln = ((CustomerEquipmentDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new CustomerEquipmentDataTable();
            }
            
            internal void InitVars() {
                this.columnSERIAL_NUMBER = this.Columns["SERIAL_NUMBER"];
                this.columnITEM_NUMBER = this.Columns["ITEM_NUMBER"];
                this.columnPORT_TYPE = this.Columns["PORT_TYPE"];
            }
            
            private void InitClass() {
                this.columnSERIAL_NUMBER = new DataColumn("SERIAL_NUMBER", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnSERIAL_NUMBER);
                this.columnITEM_NUMBER = new DataColumn("ITEM_NUMBER", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnITEM_NUMBER);
                this.columnPORT_TYPE = new DataColumn("PORT_TYPE", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPORT_TYPE);
            }
            
            public CustomerEquipmentRow NewCustomerEquipmentRow() {
                return ((CustomerEquipmentRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new CustomerEquipmentRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(CustomerEquipmentRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.CustomerEquipmentRowChanged != null)) {
                    this.CustomerEquipmentRowChanged(this, new CustomerEquipmentRowChangeEvent(((CustomerEquipmentRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.CustomerEquipmentRowChanging != null)) {
                    this.CustomerEquipmentRowChanging(this, new CustomerEquipmentRowChangeEvent(((CustomerEquipmentRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.CustomerEquipmentRowDeleted != null)) {
                    this.CustomerEquipmentRowDeleted(this, new CustomerEquipmentRowChangeEvent(((CustomerEquipmentRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.CustomerEquipmentRowDeleting != null)) {
                    this.CustomerEquipmentRowDeleting(this, new CustomerEquipmentRowChangeEvent(((CustomerEquipmentRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveCustomerEquipmentRow(CustomerEquipmentRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CustomerEquipmentRow : DataRow {
            
            private CustomerEquipmentDataTable tableCustomerEquipment;
            
            internal CustomerEquipmentRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableCustomerEquipment = ((CustomerEquipmentDataTable)(this.Table));
            }
            
            public string SERIAL_NUMBER {
                get {
                    try {
                        return ((string)(this[this.tableCustomerEquipment.SERIAL_NUMBERColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCustomerEquipment.SERIAL_NUMBERColumn] = value;
                }
            }
            
            public string ITEM_NUMBER {
                get {
                    try {
                        return ((string)(this[this.tableCustomerEquipment.ITEM_NUMBERColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCustomerEquipment.ITEM_NUMBERColumn] = value;
                }
            }
            
            public string PORT_TYPE {
                get {
                    try {
                        return ((string)(this[this.tableCustomerEquipment.PORT_TYPEColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCustomerEquipment.PORT_TYPEColumn] = value;
                }
            }
            
            public bool IsSERIAL_NUMBERNull() {
                return this.IsNull(this.tableCustomerEquipment.SERIAL_NUMBERColumn);
            }
            
            public void SetSERIAL_NUMBERNull() {
                this[this.tableCustomerEquipment.SERIAL_NUMBERColumn] = System.Convert.DBNull;
            }
            
            public bool IsITEM_NUMBERNull() {
                return this.IsNull(this.tableCustomerEquipment.ITEM_NUMBERColumn);
            }
            
            public void SetITEM_NUMBERNull() {
                this[this.tableCustomerEquipment.ITEM_NUMBERColumn] = System.Convert.DBNull;
            }
            
            public bool IsPORT_TYPENull() {
                return this.IsNull(this.tableCustomerEquipment.PORT_TYPEColumn);
            }
            
            public void SetPORT_TYPENull() {
                this[this.tableCustomerEquipment.PORT_TYPEColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CustomerEquipmentRowChangeEvent : EventArgs {
            
            private CustomerEquipmentRow eventRow;
            
            private DataRowAction eventAction;
            
            public CustomerEquipmentRowChangeEvent(CustomerEquipmentRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public CustomerEquipmentRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
