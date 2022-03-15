using Npgsql;

namespace BDProjektWinForms {
    public class Orders {
        public static async Task Insert( object store, NpgsqlConnection conn ) {
            if (store != null) {
                try {
                    var storeId = store.ToString().Split("/")[0];
                    var paymentDate = DateOnly.FromDateTime(DateTime.Now).AddDays(14);
                    var orderDate = DateOnly.FromDateTime(DateTime.Now).AddDays(7);
                    
                    await using var cmd = new NpgsqlCommand(
                        $"INSERT INTO \"ProjektBD\".orders VALUES( " +
                        $"default, '{paymentDate}', 0, 0.0, '{orderDate}', {storeId}, 0)", conn);
                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show("Otwarto zamówienie");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać sklep");
            }
        }

        [Obsolete]
        public static async Task Show( DataGridView dView, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand(
                    "SELECT orderid, storeid, orderdate, orderstatus, price, paymentdate, paymentstatus " +
                    "FROM \"ProjektBD\".orders ORDER BY orderid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        reader.GetInt32(0).ToString(),
                        reader.GetInt32(1).ToString(),
                        reader.GetDate(2).ToString(),
                        reader.GetInt32(3) == 1 ? "Zamknięte" : "Otwarte",
                        reader.GetDouble(4).ToString("N2"),
                        reader.GetDate(5).ToString(),
                        reader.GetInt32(6) == 1 ? "Zapłacone" : "Oczekiwanie"});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task Update( object order, NpgsqlConnection conn ) {
            if (order != null) {
                var ordId = order.ToString().Split("/")[0];
                try {
                    await using var cmd = new NpgsqlCommand(
                        $"UPDATE \"ProjektBD\".orders " +
                        $"SET orderstatus=1 " +
                        $"WHERE orderId={ordId}", conn);
                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Zamknięto zamówienie dostawy o numerze: {ordId}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać dostawę");
            }
        }

        public static async Task Update2( object order, NpgsqlConnection conn ) {
            if (order != null) {
                var ordId = order.ToString().Split("/")[0];
                try {
                    await using var cmd = new NpgsqlCommand(
                        $"UPDATE \"ProjektBD\".orders " +
                        $"SET paymentstatus=1 " +
                        $"WHERE orderId={ordId}", conn);
                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Zapłacono dostawę o numerze: {ordId}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać dostawę");
            }
        }
    }
}
