using Npgsql;

namespace BDProjektWinForms {
    public class Income {
        public static async Task Show( DataGridView dView, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".dailyincome ORDER BY incomeid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        Convert.ToString(reader.GetInt32(0)),
                        reader.GetInt32(3).ToString(),
                        reader.GetDate(1).ToString(),
                        reader.GetDouble(2).ToString("N2")});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task Insert(object store, string price, NpgsqlConnection conn) {
            var storeId = "";
            if (store != null) {
                storeId = store.ToString().Split("/")[0];
            }
            if (price != "") {
                try {
                    await using var cmd = new NpgsqlCommand(
                        $"INSERT INTO \"ProjektBD\".dailyincome " +
                        $"VALUES(default, '{DateOnly.FromDateTime(DateTime.Now)}', {price}, {storeId})", conn);
                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Dodano przychód z datą: {DateOnly.FromDateTime(DateTime.Now)}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Prosze podać kwotę");
            }
        }

        public static async Task Raport( DataGridView dView, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".\"monthlyIncome_report\" ORDER BY storeid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] { Convert.ToString(reader.GetInt32(0)), reader.GetString(1), reader.GetDouble(2).ToString("N2") });
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
