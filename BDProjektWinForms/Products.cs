using Npgsql;

namespace BDProjektWinForms {
    public class Products {
        public static async Task Show(DataGridView dView, NpgsqlConnection conn) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".products ORDER BY productid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] { Convert.ToString(reader.GetInt32(0)), reader.GetDouble(1).ToString("N2"), reader.GetString(2) });
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task Insert(string proNm, string proPr, NpgsqlConnection conn) {
            if (proNm != "" && proPr != "") {
                try {
                    await using var cmd = new NpgsqlCommand($"INSERT INTO \"ProjektBD\".products VALUES(default, {proPr}, '{proNm}')", conn);
                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Udane dodanie produktu\nNazwa: {proNm}\nCena: {proPr}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wypełnić wszystkie wymagane pola");
            }
        }

        public static async Task Update(object product, string newNm, string newPr, NpgsqlConnection conn) {
            var proId = "";
            var oldPr = "";
            var oldNm = "";
            if (product != null) {
                proId = product.ToString().Split("/")[0];
                oldPr = product.ToString().Split("/")[1];
                oldNm = product.ToString().Split("/")[2];
            }
            var proPr = newPr != "" ? newPr : oldPr;
            var proNm = newNm != "" ? newNm : oldNm;

            if (proId != "") {
                try {
                    await using var cmd = new NpgsqlCommand(
                        $"UPDATE \"ProjektBD\".products " +
                        $"SET price={proPr}, name='{proNm}' " +
                        $"WHERE productid={proId}", conn);

                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Udana aktualizacja klienta\nNazwa: {oldNm}\nCena: {oldPr}\nNowa nazwa: {proNm}\nNowa cena: {proPr}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać klienta do edycji");
            }
        }
    }
}
