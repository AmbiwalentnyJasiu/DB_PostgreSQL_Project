using Npgsql;

namespace BDProjektWinForms {
    public class Stores {

        public static async Task Show( DataGridView dView, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".stores ORDER BY storeid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] { Convert.ToString(reader.GetInt32(0)), reader.GetString(1), reader.GetString(3) + " " + reader.GetString(2), reader.GetString(4) });
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }   
        }
        public static async Task Insert(string storeAd, string storeNm, string storeSnm, string storeBa, NpgsqlConnection conn) {
            try {
                await using var cmd =
                    new NpgsqlCommand($"INSERT INTO \"ProjektBD\".stores " +
                                      $"VALUES(" +
                                        $"default, " +
                                        $"'{storeAd}', " +
                                        $"'{storeNm}', " +
                                        $"'{storeSnm}', " +
                                        $"'{storeBa}')", conn);
                await cmd.ExecuteNonQueryAsync();

                MessageBox.Show($"Udane dodanie sklepu\n" +
                                $"Adres: {storeAd}\n" +
                                $"Właściciel: {storeSnm} {storeNm}\n" +
                                $"Konto bankowe: {storeBa}");
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task Update(object store, string newAd, string newNm, string newSnm, string newBa, NpgsqlConnection conn) {
            var storeId = "";
            var oldAd   = "";
            var oldNm   = "";
            var oldSnm  = "";
            var oldBa   = "";
            if (store != null) {
                storeId = store.ToString().Split("/")[0];
                oldAd   = store.ToString().Split("/")[1];
                oldNm   = store.ToString().Split("/")[2].Split("_")[1];
                oldSnm  = store.ToString().Split("/")[2].Split("_")[0];
                oldBa   = store.ToString().Split("/")[3];
            }
            var storeAd  = newAd != "" ? newAd : oldAd;
            var storeNm  = newNm != "" ? newNm : oldNm;
            var storeSnm = newSnm != "" ? newSnm : oldSnm;
            var storeBa  = newBa != "" ? newBa : oldBa;

            if (storeId != "") {
                try {
                    await using var cmd = new NpgsqlCommand(
                        $"UPDATE \"ProjektBD\".stores " +
                        $"SET address='{storeAd}', ownername='{storeNm}', " +
                        $"ownersurname='{storeSnm}', " +
                        $"bankaccountnumber='{storeBa}' " +
                        $"WHERE storeid={storeId}", conn);
                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show($"Udana aktualizacja sklepu\n" +
                                    $"Adres: {oldAd}\n" +
                                    $"Właściciel: {oldSnm} {oldNm}\n" +
                                    $"Konto bankowe: {oldBa}\n" +
                                    $"Nowy adres: {storeAd}\n" +
                                    $"Nowy właściciel: {storeSnm} {storeNm}\n" +
                                    $"Nowe konto: {storeBa}");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                MessageBox.Show("Proszę wybrać sklep do edycji");
            }
        }
    }
}
