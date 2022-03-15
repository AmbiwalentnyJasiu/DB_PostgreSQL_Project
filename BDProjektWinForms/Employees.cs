using Npgsql;

namespace BDProjektWinForms {
    public class Employees {
        static public async Task Show( DataGridView dView, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".employees ORDER BY employeeid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] { Convert.ToString(reader.GetInt32(0)), Convert.ToString(reader.GetInt32(3)), reader.GetString(1), reader.GetString(2) });
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        static public async Task Insert(object store, string empNm, string empSnm, NpgsqlConnection conn ) {
            var storeId = "";
            if (store != null) {
                storeId = store.ToString().Split("/")[0];
            }
            if(storeId != "") {
                if (empNm != "" && empSnm != "") {
                    try {
                        await using var cmd = new NpgsqlCommand($"INSERT INTO \"ProjektBD\".employees VALUES(default, '{empNm}', '{empSnm}', {storeId})", conn);
                        await cmd.ExecuteNonQueryAsync();

                        MessageBox.Show($"Udane dodanie klienta\nImię: {empNm}\nNazwisko: {empSnm}\nDo sklepu: {storeId}");
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                } else {
                    MessageBox.Show("Proszę wypełnić wszystkie wymagane pola");
                }
            } else {
                MessageBox.Show("Proszę wybrać sklep");
            }
        }

        static public async Task Update( object employee, string newNm, string newSnm, NpgsqlConnection conn ) {
            var empId = "";
            var oldNm = "";
            var oldSnm = "";
            if (employee != null) {
                empId = employee.ToString().Split("/")[0];
                oldNm = employee.ToString().Split("/")[1];
                oldSnm = employee.ToString().Split("/")[2];
            }
            var empNm = newNm != "" ? newNm : oldNm;
            var empSnm = newSnm != "" ? newSnm : oldSnm;

            if (empId != "") {
                try {
                    await using var cmd = new NpgsqlCommand(
                        $"UPDATE \"ProjektBD\".employees " +
                        $"SET name='{empNm}', surname='{empSnm}' " +
                        $"WHERE employeeid={empId}", conn);

                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Udana aktualizacja klienta\n" +
                                    $"Imię: {oldNm}\n" +
                                    $"Nazwisko: {oldSnm}\n" +
                                    $"Nowe imię: {empNm}\n" +
                                    $"Nowe nazwisko: {empSnm}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać pracownika do edycji");
            }
        }

        public static async Task Raport(DataGridView dView, NpgsqlConnection conn) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".employees_report ORDER BY employeeid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] { 
                        Convert.ToString(reader.GetInt32(0)), reader.GetString(1), reader.GetString(2), reader.GetDouble(3).ToString("N2") });
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
