using Npgsql;

namespace BDProjektWinForms {
    public class Customers {
        static public async Task Show(DataGridView dView, NpgsqlConnection conn) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".customers ORDER BY customerid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] { Convert.ToString(reader.GetInt32(0)), reader.GetString(1), reader.GetString(2) });
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        static public async Task Insert(string custNm, string custSnm, NpgsqlConnection conn) {
            if (custNm != "" && custSnm != "") {
                try {
                    await using var cmd = new NpgsqlCommand($"INSERT INTO \"ProjektBD\".customers VALUES(default, '{custNm}', '{custSnm}')", conn);
                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Udane dodanie klienta\nImię: {custNm}\nNazwisko: {custSnm}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wypełnić wszystkie wymagane pola");
            }
        }

        static public async Task Update(object customer, string newNm, string newSnm, NpgsqlConnection conn) {
            var custId = "";
            var oldNm  = "";
            var oldSnm = "";
            if (customer != null) {
                custId = customer.ToString().Split("/")[0];
                oldNm  = customer.ToString().Split("/")[1];
                oldSnm = customer.ToString().Split("/")[2];
            }
            var custNm  = newNm != "" ? newNm : oldNm;
            var custSnm = newSnm != "" ? newSnm : oldSnm;

            if (custId != "") {
                try {
                    await using var cmd = new NpgsqlCommand(
                        $"UPDATE \"ProjektBD\".customers " +
                        $"SET name='{custNm}', surname='{custSnm}' " +
                        $"WHERE customerid={custId}", conn);

                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Udana aktualizacja klienta\n" +
                                    $"Imię: {oldNm}\n" +
                                    $"Nazwisko: {oldSnm}\n" +
                                    $"Nowe imię: {custNm}\n" +
                                    $"Nowe nazwisko: {custSnm}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać klienta do edycji");
            }
        }

        public static async Task Raport(DataGridView dView, NpgsqlConnection conn) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".customers_report ORDER BY customerid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] { Convert.ToString(reader.GetInt32(0)), reader.GetString(1), reader.GetString(2), reader.GetInt32(3).ToString() });
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
