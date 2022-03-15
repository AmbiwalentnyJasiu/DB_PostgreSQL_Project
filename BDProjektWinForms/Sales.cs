using Npgsql;
using System.Globalization;

namespace BDProjektWinForms {
    public class Sales {
        public static async Task Insert( string amountStr, object product, object receipt, NpgsqlConnection conn ) {
            string productNm, receiptId, productId, storeId;
            int productAm;
            double receiptVal, productVal;
            if (int.TryParse(amountStr, out var amount)) {
                if (product != null && receipt != null) {
                    productId = product.ToString().Split("/")[0];
                    receiptId = receipt.ToString().Split("/")[0];
                    productAm = Convert.ToInt32(product.ToString().Split("/")[2]);
                    productVal = Convert.ToDouble(product.ToString().Split("/")[3]);
                    productNm = product.ToString().Split("/")[2];
                    receiptVal = Convert.ToDouble(receipt.ToString().Split("/")[1]);
                    storeId = product.ToString().Split("/")[4];

                    if(productAm >= amount) {
                        var newRval = receiptVal + Convert.ToDouble(amount) * productVal;

                        try {
                            await using var cmd = new NpgsqlCommand($"INSERT INTO \"ProjektBD\".soldproducts " +
                                                                    $"VALUES( {productId}, {receiptId}, {amount})", conn);
                            await cmd.ExecuteNonQueryAsync();

                            await using var cmd2 = new NpgsqlCommand($"UPDATE \"ProjektBD\".receipts " +
                                                                     $"SET price={newRval.ToString(CultureInfo.InvariantCulture)} " +
                                                                     $"WHERE receiptId={receiptId}", conn);
                            await cmd2.ExecuteNonQueryAsync();

                            await using var cmd3 = new NpgsqlCommand($"UPDATE \"ProjektBD\".storeproducts " +
                                                                     $"SET availableamount={productAm - amount} " +
                                                                     $"WHERE productid={productId} AND storeid={storeId}", conn);
                            await cmd3.ExecuteNonQueryAsync();

                            MessageBox.Show($"Udane dodanie produktu: {productNm}\n" +
                                            $"do paragonu o numerze: {receiptId}\n" +
                                            $"w ilości: {amount}");
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message);
                        }
                    } else {
                        MessageBox.Show($"Maksymalna dostępna ilość tego produktu wynosi: {productAm}");
                    }
                    
                } else {
                    MessageBox.Show("Proszę wybrać klienta i pracownika");
                }
            } else {
                MessageBox.Show("Proszę podać prawidłową kwotę");
            }
        }

        public static async Task ShowAll( DataGridView dView, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".soldproducts_view ORDER BY receiptid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        Convert.ToString(reader.GetInt32(0)),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString(),
                        reader.GetDouble(3).ToString("N2")});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task ShowOne( DataGridView dView, string id, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".soldproducts_view " +
                                                        "WHERE receiptid=" + id + " " +
                                                        "ORDER BY name", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        Convert.ToString(reader.GetInt32(0)),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString(),
                        reader.GetDouble(3).ToString("N2")});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
