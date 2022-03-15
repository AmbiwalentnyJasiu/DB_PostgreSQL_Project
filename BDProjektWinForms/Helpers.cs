using Npgsql;
using System.Globalization;

namespace BDProjektWinForms {
    public class Helpers {

        public static async Task CustomersCombo( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".customers ORDER BY customerid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++, $"{reader.GetInt32(0)}/ {reader.GetString(1)}/ {reader.GetString(2)}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task ProductsCombo( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".products ORDER BY productid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++, $"{reader.GetInt32(0)}/ {reader.GetDouble(1):N2}/ {reader.GetString(2)}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task StoreProductsCombo(ComboBox cmb, string id, NpgsqlConnection conn ) {
            int storeId = 0;
            try {
                await using var cmd = new NpgsqlCommand($"SELECT storeid FROM \"ProjektBD\".\"receiptStore_view\" where receiptid={id}", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    storeId = reader.GetInt32(0);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            if (storeId != 0) {
                try {
                    await using var cmd = new NpgsqlCommand(
                        "SELECT productid, name, availableamount, price, storeid " +
                        "FROM \"ProjektBD\".storeproducts_view " +
                        $"WHERE storeid={storeId}", conn);
                    await using var reader = await cmd.ExecuteReaderAsync();

                    var iterator = 0;
                    cmb.Items.Clear();
                    while (await reader.ReadAsync()) {
                        cmb.Items.Insert(iterator++, $"{reader.GetInt32(0)}/ {reader.GetString(1)}/ {reader.GetInt32(2)}/ {reader.GetDouble(3):N2}/ {reader.GetInt32(4)}");
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static async Task EmployeesCombo( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".employees ORDER BY employeeid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++, $"{reader.GetInt32(0)}/ {reader.GetInt32(3)}/ {reader.GetString(1)} {reader.GetString(2)}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task StoresCombo( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".stores ORDER BY storeid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++,
                        $"{reader.GetInt32(0)}/ " +
                        $"{reader.GetString(1)}/ " +
                        $"{reader.GetString(3)}_{reader.GetString(2)}/ " +
                        $"{reader.GetString(4)}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        [Obsolete]
        public static async Task ReceiptsCombo( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".receipts_view ORDER BY receiptid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++,
                        $"{reader.GetInt32(0)}/ " +
                        $"{reader.GetDouble(1):N2}/ " +
                        $"{reader.GetDate(2)}/ " +
                        $"{(reader.GetInt32(3) == 1 ? "Zapłacony" : "Oczekiwanie")}/ " +
                        $"{reader.GetString(4)} {reader.GetString(5)}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task<int> GetStore(string recId, NpgsqlConnection conn ) {
            int storeId = 0;
            await using var cmd2 = new NpgsqlCommand($"SELECT storeid FROM \"ProjektBD\".\"receiptStore_view\" where receiptid={recId}", conn);
            await using var reader = await cmd2.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                storeId = reader.GetInt32(0);
            }

            return storeId;
        }

        [Obsolete]
        public static async Task ReceiptsCombo2( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".receipts " +
                                                        $"WHERE paymentstatus=0 " +
                                                        "ORDER BY receiptid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++,
                        $"{reader.GetInt32(0)}/ " +
                        $"{reader.GetDouble(1):N2}/ " +
                        $"{reader.GetDate(2)}/ " +
                        $"{(reader.GetInt32(3) == 1 ? "Zapłacony" : "Oczekiwanie")}/ " +
                        $"{reader.GetInt32(4)}/ " +
                        $"{reader.GetInt32(5)}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        [Obsolete]
        public static async Task OrdersCombo( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand(
                    "SELECT orderid, storeid, orderdate, orderstatus, price, paymentdate, paymentstatus " +
                    "FROM \"ProjektBD\".orders ORDER BY orderid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++,
                        $"{reader.GetInt32(0)}/ " +
                        $"{reader.GetInt32(1)}/ " +
                        $"{reader.GetDate(2)}/ " +
                        $"{(reader.GetInt32(3) == 1 ? "Zamknięte" : "Otwarte")}/ " +
                        $"{reader.GetDouble(4):N2}/ " +
                        $"{reader.GetDate(5)}/ " +
                        $"{(reader.GetInt32(6) == 1 ? "Zapłacone" : "Oczekiwanie")}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        [Obsolete]
        public static async Task OrdersCombo2( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand(
                    "SELECT orderid, storeid, orderdate, orderstatus, price, paymentdate, paymentstatus " +
                    "FROM \"ProjektBD\".orders " +
                    "WHERE orderstatus=0" +
                    "ORDER BY orderid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++,
                        $"{reader.GetInt32(0)}/ " +
                        $"{reader.GetInt32(1)}/ " +
                        $"{reader.GetDate(2)}/ " +
                        $"{(reader.GetInt32(3) == 1 ? "Zamknięte" : "Otwarte")}/ " +
                        $"{reader.GetDouble(4):N2}/ " +
                        $"{reader.GetDate(5)}/ " +
                        $"{(reader.GetInt32(6) == 1 ? "Zapłacone" : "Oczekiwanie")}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        [Obsolete]
        public static async Task OrdersCombo3( ComboBox cmb, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand(
                    "SELECT orderid, storeid, orderdate, orderstatus, price, paymentdate, paymentstatus " +
                    "FROM \"ProjektBD\".orders " +
                    "WHERE orderstatus=1 AND paymentstatus=0" +
                    "ORDER BY orderid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                cmb.Items.Clear();
                while (await reader.ReadAsync()) {
                    cmb.Items.Insert(iterator++,
                        $"{reader.GetInt32(0)}/ " +
                        $"{reader.GetInt32(1)}/ " +
                        $"{reader.GetDate(2)}/ " +
                        $"{(reader.GetInt32(3) == 1 ? "Zamknięte" : "Otwarte")}/ " +
                        $"{reader.GetDouble(4):N2}/ " +
                        $"{reader.GetDate(5)}/ " +
                        $"{(reader.GetInt32(6) == 1 ? "Zapłacone" : "Oczekiwanie")}");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
