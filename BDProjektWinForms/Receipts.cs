using Npgsql;
using System.Globalization;

namespace BDProjektWinForms {
    public class Receipts {
        public static async Task Insert(object customer, object employee, DateOnly date, NpgsqlConnection conn ) {
            var customerId = "";
            var employeeId = "";
            if (customer != null && employee != null) {
                customerId = customer.ToString().Split("/")[0];
                employeeId = employee.ToString().Split("/")[0];
            
                try {
                    await using var cmd = new NpgsqlCommand($"INSERT INTO \"ProjektBD\".receipts VALUES(default, 0.0, '{date}', 0, {employeeId}, {customerId})", conn);
                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Udane dodanie paragonu z datą: {date}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać klienta i pracownika");
            }
        }

        [Obsolete]
        public static async Task Show( DataGridView dView, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".receipts ORDER BY receiptid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        reader.GetInt32(0).ToString(),
                        reader.GetDouble(1).ToString("N2"),
                        reader.GetDate(2).ToString(),
                        reader.GetInt32(3) == 1 ? "Zapłacono" : "Oczekiwanie",
                        reader.GetInt32(4).ToString(),
                        reader.GetInt32(5).ToString()});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task Update( object receipt, NpgsqlConnection conn ) {
            if (receipt != null) {
                var recId = receipt.ToString().Split("/")[0];
                var recPrice = Convert.ToDouble(receipt.ToString().Split("/")[1]);
                try {
                    await using var cmd = new NpgsqlCommand(
                        $"UPDATE \"ProjektBD\".receipts " +
                        $"SET paymentstatus=1 " +
                        $"WHERE receiptId={recId}", conn);
                    await cmd.ExecuteNonQueryAsync();


                    int storeId = await Helpers.GetStore(recId, conn);

                    MessageBox.Show(storeId.ToString());
                    


                    await using var cmdint = new NpgsqlCommand($"INSERT INTO \"ProjektBD\".dailyincome VALUES( " +
                                                             $"default, '{DateOnly.FromDateTime(DateTime.Now)}', " +
                                                             $"{recPrice.ToString(CultureInfo.InvariantCulture)}, " +
                                                             $"{storeId})", conn);
                    await cmdint.ExecuteNonQueryAsync();

                    MessageBox.Show($"Zamknięto zamówienie o numerze: {recId}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać zamówienie");
            }
        }
    }
}
