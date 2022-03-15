using Npgsql;

namespace BDProjektWinForms {
    public partial class Form1 : Form {
        private string connString = "";
        private NpgsqlConnection conn;

        public Form1() {
            InitializeComponent();

        }

        ///LOGOWANIE

        /// <summary>
        /// Funckja sprawdza czy logowanie do bazy było udane i otwiera połączenie jeśli dane są poprawne
        /// </summary>
        private async void loginBtn_Click( object sender, EventArgs e ) {
            var uname = this.uNameFld.Text;
            var pass =  this.passFld.Text;
            var dbaseName = this.DBase.Text;
            var hostName = this.Host.Text;

            this.connString = $"Host={hostName};Username={uname};Password={pass};Database={dbaseName}";

            try {
                this.conn = new NpgsqlConnection(this.connString);
                await this.conn.OpenAsync();

                MessageBox.Show("Poprawnie zalogowano!");

                this.tabControl.Visible = true;
                this.tabControl.BringToFront();
                this.loginPnl.Hide();
            } catch (Exception ex) {
                MessageBox.Show($"Nieprawidłowe dane logowania!\n\n\nDetails:  {ex.Message}");
            }
        }




        ///OBSŁUGA TABELI CUSTOMERS

        /// <summary>
        /// Dodawanie do tablicy klientów           INSERT
        /// </summary>
        private async void cusInsButt_Click( object sender, EventArgs e ) {
            var custNm =  this.cusInsName.Text;
            var custSnm = this.cusInsSur.Text;

            await Customers.Insert(custNm, custSnm, this.conn);
        }

        /// <summary>
        /// Aktualizowanie rekordu w tablicy klientów           UPDATE
        /// </summary>
        private async void cusUpdButt_Click( object sender, EventArgs e ) {
            var customer = this.custUpdCmb.SelectedItem;
            await Customers.Update(customer, this.cusUpdNm.Text, this.cusUpdSnm.Text, this.conn);
        }

        /// <summary>
        /// Załadowanie danych do combobox w formularzu aktualizacji
        /// </summary>
        private async void custUpdCmb_Click( object sender, EventArgs e ) {
            await Helpers.CustomersCombo(this.custUpdCmb, this.conn);
        }

        /// <summary>
        /// Funkcja wyświetla w obiekcie dataGrid zawartośc tabeli klientów         SELECT
        /// </summary>
        private async void custViewButton_Click( object sender, EventArgs e ) {
            await Customers.Show(this.custDataView, this.conn);
        }





        ///OBSŁUGA TABELI PRODUCTS

        /// <summary>
        /// Dodawanie wpisu do tabeli produktów         INSERT
        /// </summary>
        private async void proInsButt_Click( object sender, EventArgs e ) {
            var proPr = this.proInsPr.Text;
            var proNm = this.proInsNm.Text;

            await Products.Insert(proNm, proPr, this.conn);
        }

        /// <summary>
        /// Aktualizowanie rekordu w tablicy klientów           UPDATE
        /// </summary>
        private async void proUpdButt_Click( object sender, EventArgs e ) {
            var product = this.proUpdCmb.SelectedItem;
            await Products.Update(product, this.proUpdNm.Text, this.proUpdPr.Text, this.conn);
        }

        /// <summary>
        /// Załadowanie danych do combobox w formularzu aktualizacji
        /// </summary>
        private async void proUpdCmb_Click( object sender, EventArgs e ) {
            await Helpers.ProductsCombo(this.proUpdCmb, this.conn);
        }

        /// <summary>
        /// Funkcja wyświetla w obiekcie dataGrid zawartośc tabeli klientów         SELECT
        /// </summary>
        private async void proViewButton_Click( object sender, EventArgs e ) {
            await Products.Show(this.proDataView, this.conn);
        }




        ///OBSŁUGA TABELI STORES

        /// <summary>
        /// Dodawanie rekordu do tabeli sklepów         INSERT
        /// </summary>
        private async void storeInsButt_Click( object sender, EventArgs e ) {
            var storeAd = this.storeInsAd.Text;
            var storeNm = this.storeInsNm.Text;
            var storeSnm = this.storeInsSnm.Text;
            var storeBa = this.storeInsBan.Text;

            await Stores.Insert(storeAd, storeNm, storeSnm, storeBa, this.conn);
        }

        private async void storeUpdButt_Click( object sender, EventArgs e ) {
            var store = this.storeUpdCmb.SelectedItem;
            await Stores.Update(store, this.storeUpdAd.Text, this.storeUpdNm.Text,this.storeUpdSnm.Text, this.storeUpdBa.Text, this.conn);
        }

        private async void storeUpdCmb_Click( object sender, EventArgs e ) {
            await Helpers.StoresCombo(this.storeUpdCmb, this.conn);
        }

        private async void storeViewButton_Click( object sender, EventArgs e ) {
            await Stores.Show(this.storeDataView, this.conn);
        }

        
        /// OBSŁUGA TABELI EMPLOYEES

        private async void empInsButt_Click( object sender, EventArgs e ) {
            var store = this.empInsCmb.SelectedItem;
            var empNm = this.empInsNm.Text;
            var empSnm = this.empInsSnm.Text;

            await Employees.Insert(store, empNm, empSnm, this.conn);
        }

        private async void empUpdButt_Click( object sender, EventArgs e ) {
            var employee = this.empUpdCmb.SelectedItem;
            await Employees.Update(employee, this.empUpdNm.Text, this.empUpdSnm.Text, this.conn);
        }

        private async void empUpdCmb_Click( object sender, EventArgs e ) {
            await Helpers.EmployeesCombo(this.empUpdCmb, this.conn);
        }

        private async void empInsCmb_Click( object sender, EventArgs e ) {
            await Helpers.StoresCombo(this.empInsCmb, this.conn);
        }

        private async void empViewButton_Click( object sender, EventArgs e ) {
            await Employees.Show(this.empDataView, this.conn);
        }


        ///OBSŁUGA TABELI RECEIPTS

        [Obsolete]
        private async void reUpdCmb_Click( object sender, EventArgs e ) {
            await Helpers.ReceiptsCombo2(this.reUpdCmb, this.conn);
        }

        private async void reInsCCmb_Click( object sender, EventArgs e ) {
            await Helpers.CustomersCombo(this.reInsCCmb, this.conn);
        }

        private async void reInsECmb_Click( object sender, EventArgs e ) {
            await Helpers.EmployeesCombo(this.reInsECmb, this.conn);
        }

        private async void reInsButt_Click( object sender, EventArgs e ) {
            var date = DateOnly.FromDateTime(DateTime.Now);
            var customer = this.reInsCCmb.SelectedItem;
            var employee = this.reInsECmb.SelectedItem;

            await Receipts.Insert(customer, employee, date, this.conn);
        }

        [Obsolete]
        private async void reViewButt_Click( object sender, EventArgs e ) {
            await Receipts.Show(this.reDataView, this.conn);
        }

        private async void reUpdButt_Click( object sender, EventArgs e ) {
            var receipt = this.reUpdCmb.SelectedItem;
            await Receipts.Update(receipt, this.conn);
        }

        /// OBSŁUGA TABELI DAILYINCOME

        private async void incInsCmb_Click( object sender, EventArgs e ) {
            await Helpers.StoresCombo(this.incInsCmb, this.conn);
        }

        private async void incInsButt_Click( object sender, EventArgs e ) {
            var price = this.incInsPr.Text;
            var store = this.incInsCmb.SelectedItem;

            await Income.Insert(store, price, this.conn);
        }

        private async void incViewButt_Click( object sender, EventArgs e ) {
            await Income.Show(this.incDataView, this.conn);
        }

        /// OBSŁUGA TABELI SOLDPRODUCTS

        private async void salInsProCmb_Click( object sender, EventArgs e ) {
            var receipt = this.salInsParCmb.SelectedItem;
            if(receipt != null) {
                var receiptId = receipt.ToString().Split("/")[0];
                await Helpers.StoreProductsCombo(this.salInsProCmb, receiptId, this.conn);
            } else {
                MessageBox.Show("Proszę wybrać paragon");
            }
        }

        [Obsolete]
        private async void salInsParCmb_Click( object sender, EventArgs e ) {
            await Helpers.ReceiptsCombo2(this.salInsParCmb, this.conn);
        }

        private async void salInsButt_Click( object sender, EventArgs e ) {
            var amountStr = this.salInsAm.Text;
            var product = this.salInsProCmb.SelectedItem;
            var receipt = this.salInsParCmb.SelectedItem;

            await Sales.Insert(amountStr, product, receipt, this.conn);
        }

        [Obsolete]
        private async void salViewCmb_Click( object sender, EventArgs e ) {
            await Helpers.ReceiptsCombo(this.salViewCmb, this.conn);
            this.salViewCmb.Items.Insert(0, "Wszystkie");
        }

        private async void salViewButt_Click( object sender, EventArgs e ) {
            var receiptObj = this.salViewCmb.SelectedItem;
            if(receiptObj != null) {
                var receipt = receiptObj.ToString();
                if (receipt.Equals("Wszystkie")) {
                    await Sales.ShowAll(this.salDataView, this.conn);
                } else {
                    var receiptId = receipt.Split("/")[0];
                    await Sales.ShowOne(this.salDataView, receiptId, this.conn);
                }
            } else {
                await Sales.ShowAll(this.salDataView, this.conn);
            }
        }



        /// OBSŁYGA TABELI STOREPRODUCTS

        private async void stockInsSr_Click( object sender, EventArgs e ) {
            await Helpers.StoresCombo(this.stockInsSr, this.conn);
        }

        private async void stockInsPr_Click( object sender, EventArgs e ) {
            await Helpers.ProductsCombo(this.stockInsPr, this.conn);
        }

        private async void stockInsButt_Click( object sender, EventArgs e ) {
            var store = this.stockInsSr.SelectedItem;
            var product = this.stockInsPr.SelectedItem;
            var amount = this.stockInsAm.Text;

            await Stock.Insert(store, product, amount, this.conn);
        }

        private async void stockViewCmb_Click( object sender, EventArgs e ) {
            await Helpers.StoresCombo(this.stockViewCmb, this.conn);
            this.stockViewCmb.Items.Insert(0, "Wszystkie");
        }

        private async void stockViewButt_Click( object sender, EventArgs e ) {
            var storeObj = this.stockViewCmb.SelectedItem;
            if (storeObj != null) {
                var store = storeObj.ToString();
                if (store.Equals("Wszystkie")) {
                    await Stock.ShowAll(this.stockDataView, this.conn);
                } else {
                    var storeId = store.Split("/")[0];
                    await Stock.ShowOne(this.stockDataView, storeId, this.conn);
                }
            } else {
                await Stock.ShowAll(this.stockDataView, this.conn);
            }
        }

        private async void stockUpdSt_Click( object sender, EventArgs e ) {
            await Helpers.StoresCombo(this.stockUpdSr, this.conn);
        }

        private async void stockUpdPr_Click( object sender, EventArgs e ) {
            await Helpers.ProductsCombo(this.stockUpdPr, this.conn);
        }

        private async void stockUpdButt_Click( object sender, EventArgs e ) {
            var store = this.stockUpdSr.SelectedItem;
            var product = this.stockUpdPr.SelectedItem;
            var amount = this.stockUpdAm.Text;

            await Stock.Update(store, product, amount, this.conn);
        }

        

        /// OBSŁUGA TABELI ORDERS

        private async void ordInsSr_Click( object sender, EventArgs e ) {
            await Helpers.StoresCombo(this.ordInsSr, this.conn);
        }

        private async void ordInsButt_Click( object sender, EventArgs e ) {
            var store = this.ordInsSr.SelectedItem;

            await Orders.Insert(store, this.conn);
        }

        [Obsolete]
        private async void ordViewButt_Click( object sender, EventArgs e ) {
            await Orders.Show(this.ordDataView, this.conn);
        }

        [Obsolete]
        private async void ordUpdCmb_Click( object sender, EventArgs e ) {
            await Helpers.OrdersCombo2(this.ordUpdCmb, this.conn);
        }

        private async void ordUpdButt_Click( object sender, EventArgs e ) {
            var order = this.ordUpdCmb.SelectedItem;

            await Orders.Update(order, this.conn);
        }

        [Obsolete]
        private async void ordPayCmb_Click( object sender, EventArgs e ) {
            await Helpers.OrdersCombo3(this.ordPayCmb, this.conn);
        }

        private async void ordPayButt_Click( object sender, EventArgs e ) {
            var order = this.ordPayCmb.SelectedItem;

            await Orders.Update2(order, this.conn);
        }

        private async void ordListInsPr_Click( object sender, EventArgs e ) {
            await Helpers.ProductsCombo(this.ordListInsPr, this.conn);
        }

        [Obsolete]
        private async void ordListInsOr_Click( object sender, EventArgs e ) {
            await Helpers.OrdersCombo2(this.ordListInsOr, this.conn);
        }

        private async void ordListInsButt_Click( object sender, EventArgs e ) {
            var product = this.ordListInsPr.SelectedItem;
            var order = this.ordListInsOr.SelectedItem;
            var amount = this.ordListInsAm.Text;

            await OrderList.Insert(product, order, amount, this.conn);
        }

        private async void ordListViewCmb_Click( object sender, EventArgs e ) {
            await Helpers.StoresCombo(this.ordListViewCmb, this.conn);
            this.ordListViewCmb.Items.Insert(0, "Wszystkie");
        }

        private async void ordListViewButt_Click( object sender, EventArgs e ) {
            var storeObj = this.ordListViewCmb.SelectedItem;
            if (storeObj != null) {
                var store = storeObj.ToString();
                if (store.Equals("Wszystkie")) {
                    await OrderList.ShowAll(this.ordListDataView, this.conn);
                } else {
                    var storeId = store.Split("/")[0];
                    await OrderList.ShowOne(this.ordListDataView, storeId, this.conn);
                }
            } else {
                await OrderList.ShowAll(this.ordListDataView, this.conn);
            }
        }

        private async void custRaportButt_Click( object sender, EventArgs e ) {
            await Customers.Raport(this.custRaportDataView, this.conn);
        }

        private async void empRapButt_Click( object sender, EventArgs e ) {
            await Employees.Raport(this.empRapView, this.conn);
        }

        private async void incRapButt_Click( object sender, EventArgs e ) {
            await Income.Raport(this.incRapView, this.conn);
        }
    }
}