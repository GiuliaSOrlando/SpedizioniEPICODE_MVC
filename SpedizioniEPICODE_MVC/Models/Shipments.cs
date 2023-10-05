using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpedizioniEPICODE_MVC.Models;

namespace SpedizioniEPICODE_MVC.Models
{
    public class Shipments
    {
        //Proprietà
        public int IdSpedizione { get; set; }


        [Display(Name = "Numero identificativo")]
        [StringLength(36)]
        public string NumeroIdentificativo { get; set; }

        [Required(ErrorMessage = "Il campo data spedizione è obbligatorio.")]
        [Display(Name = "Data spedizione")]
        [DataType(DataType.Date)]
        public DateTime DataSpedizione { get; set; }

        [Required(ErrorMessage = "Il campo peso è obbligatorio.")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Peso")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il campo peso deve essere maggiore di zero.")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Il campo città destinataria è obbligatorio.")]
        [Display(Name = "Città del destinatario")]
        [StringLength(100)]
        public string CittaDestinataria { get; set; }

        [Required(ErrorMessage = "Il campo indirizzo destinatario è obbligatorio.")]
        [Display(Name = "Indirizzo del destinatario")]
        [StringLength(100)]
        public string IndirizzoDestinatario { get; set; }

        [Required(ErrorMessage = "Il campo nominativo destinatario è obbligatorio.")]
        [Display(Name = "Nominativo del destinatario")]
        [StringLength(100)]
        public string NominativoDestinatario { get; set; }

        [Required(ErrorMessage = "Il campo costo spedizione è obbligatorio.")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Costo spedizione")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il campo costo spedizione deve essere maggiore di zero.")]
        public decimal CostoSpedizione { get; set; }

        [Required(ErrorMessage = "Il campo data consegna Prevista è obbligatorio.")]
        [Display(Name = "Data prevista della consegna")]
        [DataType(DataType.Date)]
        public DateTime DataConsegnaPrevista { get; set; }

        [Required(ErrorMessage = "Il campo stato spedizione è obbligatorio.")]
        [Display(Name = "Stato spedizione")]
        [StringLength(20)]
        public string StatoSpedizione { get; set; }

        [Display(Name = "Luogo attuale")]
        [StringLength(100)]
        public string LuogoAttuale { get; set; }

        [Display(Name = "Descrizione")]
        public string DescrizioneAggiornamento { get; set; }

        [Required(ErrorMessage = "Il campo data ora aggiornamento è obbligatorio.")]
        [Display(Name = "Data dell'aggiornamento")]
        [DataType(DataType.DateTime)]
        public DateTime DataOraAggiornamento { get; set; }

        [Required(ErrorMessage = "Il campo cliente ID è obbligatorio.")]
        [Display(Name = "Identificativo cliente")]
        public int ClienteID { get; set; }

        public Customers Cliente { get; set; }

        public string Nome { get; set; }
        public string Cognome { get; set; }

        //Metodi
        public bool CreaSpedizione()
        {
            NumeroIdentificativo = Guid.NewGuid().ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand inserisciSpedizione = new SqlCommand("INSERT INTO Spedizioni(NumeroIdentificativo, DataSpedizione, Peso, CittaDestinataria, IndirizzoDestinatario," +
                    "NominativoDestinatario, CostoSpedizione, DataConsegnaPrevista, StatoSpedizione, LuogoAttuale, DescrizioneAggiornamento, DataOraAggiornamento," +
                    "ClienteID) VALUES(@NumeroIdentificativo, @DataSpedizione, @Peso, @CittaDestinataria, @IndirizzoDestinatario, @NominativoDestinatario," +
                    "@CostoSpedizione, @DataConsegnaPrevista, @StatoSpedizione, @LuogoAttuale, @DescrizioneAggiornamento, @DataOraAggiornamento, @ClienteID)", sqlConnection);

                inserisciSpedizione.Parameters.AddWithValue("@NumeroIdentificativo", NumeroIdentificativo);
                inserisciSpedizione.Parameters.AddWithValue("@DataSpedizione", DataSpedizione);
                inserisciSpedizione.Parameters.AddWithValue("@Peso", Peso);
                inserisciSpedizione.Parameters.AddWithValue("@CittaDestinataria", CittaDestinataria);
                inserisciSpedizione.Parameters.AddWithValue("@IndirizzoDestinatario", IndirizzoDestinatario);
                inserisciSpedizione.Parameters.AddWithValue("@NominativoDestinatario", NominativoDestinatario);
                inserisciSpedizione.Parameters.AddWithValue("@CostoSpedizione", CostoSpedizione);
                inserisciSpedizione.Parameters.AddWithValue("@DataConsegnaPrevista", DataConsegnaPrevista);
                inserisciSpedizione.Parameters.AddWithValue("@StatoSpedizione", StatoSpedizione);
                inserisciSpedizione.Parameters.AddWithValue("@LuogoAttuale", LuogoAttuale);
                inserisciSpedizione.Parameters.AddWithValue("@DescrizioneAggiornamento", (object)DescrizioneAggiornamento ?? DBNull.Value);
                inserisciSpedizione.Parameters.AddWithValue("@DataOraAggiornamento", DataOraAggiornamento);
                inserisciSpedizione.Parameters.AddWithValue("@ClienteID", ClienteID);

                inserisciSpedizione.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sqlConnection.Close();
            }


            return false;
        }

        public static List<Shipments> TracciaSpedizionePerCF(string CF, string numeroSpedizione)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            List<Shipments> shipments = new List<Shipments>();

            try
            {
                sqlConnection.Open();
                SqlCommand cercaClientePerCF = new SqlCommand("SELECT IdCliente FROM Clienti WHERE CF=@Cf", sqlConnection);
                cercaClientePerCF.Parameters.AddWithValue("@CF", CF);
                int idCliente = (int)cercaClientePerCF.ExecuteScalar();

                SqlCommand verificaNumeroSpedizione = new SqlCommand(
                                        "SELECT s.*, c.Nome, c.Cognome " +
                                        "FROM Spedizioni s " +
                                        "INNER JOIN Clienti c ON s.ClienteId = c.IdCliente " +
                                        "WHERE s.ClienteId = @IdCliente AND s.NumeroIdentificativo = @NumeroIdentificativo", sqlConnection);
                verificaNumeroSpedizione.Parameters.AddWithValue("@IdCliente", idCliente);
                verificaNumeroSpedizione.Parameters.AddWithValue("@NumeroIdentificativo", numeroSpedizione);

                SqlDataReader reader = verificaNumeroSpedizione.ExecuteReader();


                while (reader.Read())
                {
                    Shipments shipment = new Shipments
                    {
                        IdSpedizione = (int)reader["IdSpedizione"],
                        NumeroIdentificativo = reader["NumeroIdentificativo"].ToString(),
                        DataSpedizione = (DateTime)reader["DataSpedizione"],
                        Peso = (decimal)reader["Peso"],
                        CittaDestinataria = reader["CittaDestinataria"].ToString(),
                        IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                        NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                        CostoSpedizione = (decimal)reader["CostoSpedizione"],
                        DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"],
                        StatoSpedizione = reader["StatoSpedizione"].ToString(),
                        LuogoAttuale = reader["LuogoAttuale"].ToString(),
                        DescrizioneAggiornamento = reader["DescrizioneAggiornamento"].ToString(),
                        DataOraAggiornamento = (DateTime)reader["DataOraAggiornamento"],
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };
                    shipments.Add(shipment);
                }
                reader.Close();

            }
            catch (Exception ex) { }
            finally { sqlConnection.Close(); }
            return shipments;
        }

        public static List<Shipments> CaricaSpedizioniDB()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            List<Shipments> shipments = new List<Shipments>();

            try
            {
                sqlConnection.Open();
                SqlCommand getAllShipmentsCommand = new SqlCommand(
                    "SELECT s.*, c.Nome, c.Cognome " +
                    "FROM Spedizioni s " +
                    "INNER JOIN Clienti c ON s.ClienteId = c.IdCliente", sqlConnection);

                SqlDataReader reader = getAllShipmentsCommand.ExecuteReader();

                while (reader.Read())
                {
                    Shipments shipment = new Shipments
                    {
                        IdSpedizione = (int)reader["IdSpedizione"],
                        NumeroIdentificativo = reader["NumeroIdentificativo"].ToString(),
                        DataSpedizione = (DateTime)reader["DataSpedizione"],
                        Peso = (decimal)reader["Peso"],
                        CittaDestinataria = reader["CittaDestinataria"].ToString(),
                        IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                        NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                        CostoSpedizione = (decimal)reader["CostoSpedizione"],
                        DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"],
                        StatoSpedizione = reader["StatoSpedizione"].ToString(),
                        LuogoAttuale = reader["LuogoAttuale"].ToString(),
                        DescrizioneAggiornamento = reader["DescrizioneAggiornamento"].ToString(),
                        DataOraAggiornamento = (DateTime)reader["DataOraAggiornamento"],
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };
                    shipments.Add(shipment);
                }
                reader.Close();
            }
            catch (Exception ex) { }
            finally { sqlConnection.Close(); }

            return shipments;
        }

        public static Shipments DettagliSpedizione(int Id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            Shipments shipment = null;

            try
            {
                sqlConnection.Open();
                SqlCommand getShipmentDetailsCommand = new SqlCommand(
                    "SELECT s.*, c.Nome, c.Cognome, s.ClienteId " +
                    "FROM Spedizioni s " +
                    "INNER JOIN Clienti c ON s.ClienteId = c.IdCliente " +
                    "WHERE s.IdSpedizione = @ShipmentId", sqlConnection);

                getShipmentDetailsCommand.Parameters.AddWithValue("@ShipmentId", Id);
                SqlDataReader reader = getShipmentDetailsCommand.ExecuteReader();

                if (reader.Read())
                {
                    shipment = new Shipments
                    {
                        IdSpedizione = (int)reader["IdSpedizione"],
                        NumeroIdentificativo = reader["NumeroIdentificativo"].ToString(),
                        DataSpedizione = (DateTime)reader["DataSpedizione"],
                        Peso = (decimal)reader["Peso"],
                        CittaDestinataria = reader["CittaDestinataria"].ToString(),
                        IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                        NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                        CostoSpedizione = (decimal)reader["CostoSpedizione"],
                        DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"],
                        StatoSpedizione = reader["StatoSpedizione"].ToString(),
                        LuogoAttuale = reader["LuogoAttuale"].ToString(),
                        DescrizioneAggiornamento = reader["DescrizioneAggiornamento"].ToString(),
                        DataOraAggiornamento = (DateTime)reader["DataOraAggiornamento"],
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString(),
                        ClienteID = (int)reader["ClienteId"]
                    };
                }

                reader.Close();
            }
            catch (Exception ex) { }
            finally { sqlConnection.Close(); }

            return shipment;
        }

        public static bool ModificaSpedizione(Shipments aggiornamentoSpedizione)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();

                SqlCommand modificaSpedizione = new SqlCommand(
                    "UPDATE Spedizioni " +
                    "SET NumeroIdentificativo = @NumeroIdentificativo, " +
                    "DataSpedizione = @DataSpedizione, " +
                    "Peso = @Peso, " +
                    "CittaDestinataria = @CittaDestinataria, " +
                    "IndirizzoDestinatario = @IndirizzoDestinatario, " +
                    "NominativoDestinatario = @NominativoDestinatario, " +
                    "CostoSpedizione = @CostoSpedizione, " +
                    "DataConsegnaPrevista = @DataConsegnaPrevista, " +
                    "StatoSpedizione = @StatoSpedizione, " +
                    "LuogoAttuale = @LuogoAttuale, " +
                    "DescrizioneAggiornamento = @DescrizioneAggiornamento, " +
                    "DataOraAggiornamento = @DataOraAggiornamento, " +
                    "ClienteId = @ClienteId " +
                    "WHERE IdSpedizione = @IdSpedizione", sqlConnection);

                modificaSpedizione.Parameters.AddWithValue("@IdSpedizione", aggiornamentoSpedizione.IdSpedizione);
                modificaSpedizione.Parameters.AddWithValue("@NumeroIdentificativo", aggiornamentoSpedizione.NumeroIdentificativo);
                modificaSpedizione.Parameters.AddWithValue("@DataSpedizione", aggiornamentoSpedizione.DataSpedizione);
                modificaSpedizione.Parameters.AddWithValue("@Peso", aggiornamentoSpedizione.Peso);
                modificaSpedizione.Parameters.AddWithValue("@CittaDestinataria", aggiornamentoSpedizione.CittaDestinataria);
                modificaSpedizione.Parameters.AddWithValue("@IndirizzoDestinatario", aggiornamentoSpedizione.IndirizzoDestinatario);
                modificaSpedizione.Parameters.AddWithValue("@NominativoDestinatario", aggiornamentoSpedizione.NominativoDestinatario);
                modificaSpedizione.Parameters.AddWithValue("@CostoSpedizione", aggiornamentoSpedizione.CostoSpedizione);
                modificaSpedizione.Parameters.AddWithValue("@DataConsegnaPrevista", aggiornamentoSpedizione.DataConsegnaPrevista);
                modificaSpedizione.Parameters.AddWithValue("@StatoSpedizione", aggiornamentoSpedizione.StatoSpedizione);
                modificaSpedizione.Parameters.AddWithValue("@LuogoAttuale", aggiornamentoSpedizione.LuogoAttuale);
                modificaSpedizione.Parameters.AddWithValue("@DescrizioneAggiornamento", (object)aggiornamentoSpedizione.DescrizioneAggiornamento ?? DBNull.Value);
                modificaSpedizione.Parameters.AddWithValue("@DataOraAggiornamento", aggiornamentoSpedizione.DataOraAggiornamento);
                modificaSpedizione.Parameters.AddWithValue("@ClienteId", aggiornamentoSpedizione.ClienteID); 

                modificaSpedizione.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sqlConnection.Close();
            }

            return false;
        }

        public static List<Shipments> VisualizzaSpedizioneFiltro(
            DateTime? dataInizio,
            DateTime? dataFine,
            string numeroIdentificativo,
            string statoSpedizione,
            string cittaDestinataria,
            decimal? pesoMinimo,
            decimal? pesoMassimo,
            DateTime? dataConsegnaPrevistaMinima,
            DateTime? dataConsegnaPrevistaMassima,
            decimal? costoMinimo,
            decimal? costoMassimo,
            string clienteNome,
            string clienteCognome)
            {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            List<Shipments> spedizioni = new List<Shipments>();

            try
            {
                sqlConnection.Open();

                string query = "SELECT Spedizioni.*, Clienti.Nome, Clienti.Cognome " +
                               "FROM Spedizioni " +
                               "INNER JOIN Clienti ON Spedizioni.ClienteID = Clienti.IdCliente ";

                List<string> conditions = new List<string>();
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (dataInizio != null)
                {
                    conditions.Add("Spedizioni.DataSpedizione >= @DataInizio");
                    parameters.Add(new SqlParameter("@DataInizio", dataInizio));
                }
                if (dataFine != null)
                {
                    conditions.Add("Spedizioni.DataSpedizione <= @DataFine");
                    parameters.Add(new SqlParameter("@DataFine", dataFine));
                }
                if (!string.IsNullOrEmpty(numeroIdentificativo))
                {
                    conditions.Add("Spedizioni.NumeroIdentificativo = @NumeroIdentificativo");
                    parameters.Add(new SqlParameter("@NumeroIdentificativo", numeroIdentificativo));
                }

                if (!string.IsNullOrEmpty(statoSpedizione))
                {
                    conditions.Add("Spedizioni.StatoSpedizione = @StatoSpedizione");
                    parameters.Add(new SqlParameter("@StatoSpedizione", statoSpedizione));
                }

                if (!string.IsNullOrEmpty(cittaDestinataria))
                {
                    conditions.Add("Spedizioni.CittaDestinataria = @CittaDestinataria");
                    parameters.Add(new SqlParameter("@CittaDestinataria", cittaDestinataria));
                }

                if (pesoMinimo != null)
                {
                    conditions.Add("Spedizioni.Peso >= @PesoMinimo");
                    parameters.Add(new SqlParameter("@PesoMinimo", pesoMinimo));
                }

                if (pesoMassimo != null)
                {
                    conditions.Add("Spedizioni.Peso <= @PesoMassimo");
                    parameters.Add(new SqlParameter("@PesoMassimo", pesoMassimo));
                }

                if (dataConsegnaPrevistaMinima != null)
                {
                    conditions.Add("Spedizioni.DataConsegnaPrevista >= @DataConsegnaPrevistaMinima");
                    parameters.Add(new SqlParameter("@DataConsegnaPrevistaMinima", dataConsegnaPrevistaMinima));
                }

                if (dataConsegnaPrevistaMassima != null)
                {
                    conditions.Add("Spedizioni.DataConsegnaPrevista <= @DataConsegnaPrevistaMassima");
                    parameters.Add(new SqlParameter("@DataConsegnaPrevistaMassima", dataConsegnaPrevistaMassima));
                }

                if (costoMinimo != null)
                {
                    conditions.Add("Spedizioni.CostoSpedizione >= @CostoMinimo");
                    parameters.Add(new SqlParameter("@CostoMinimo", costoMinimo));
                }

                if (costoMassimo != null)
                {
                    conditions.Add("Spedizioni.CostoSpedizione <= @CostoMassimo");
                    parameters.Add(new SqlParameter("@CostoMassimo", costoMassimo));
                }

                if (!string.IsNullOrEmpty(clienteNome))
                {
                    conditions.Add("Clienti.Nome = @ClienteNome");
                    parameters.Add(new SqlParameter("@ClienteNome", clienteNome));
                }

                if (!string.IsNullOrEmpty(clienteCognome))
                {
                    conditions.Add("Clienti.Cognome = @ClienteCognome");
                    parameters.Add(new SqlParameter("@ClienteCognome", clienteCognome));
                }


                if (conditions.Count > 0)
                {
                    query += "WHERE " + string.Join(" AND ", conditions);
                }

                SqlCommand visualizzaSpedizioneData = new SqlCommand(query, sqlConnection);
                visualizzaSpedizioneData.Parameters.AddRange(parameters.ToArray());

                SqlDataReader reader = visualizzaSpedizioneData.ExecuteReader();

                while (reader.Read())
                {
                    Shipments shipment = new Shipments();
                    shipment.IdSpedizione = (int)reader["IdSpedizione"];
                    shipment.NumeroIdentificativo = reader["NumeroIdentificativo"].ToString();
                    shipment.DataSpedizione = (DateTime)reader["DataSpedizione"];
                    shipment.StatoSpedizione = reader["StatoSpedizione"].ToString();
                    shipment.CittaDestinataria = reader["CittaDestinataria"].ToString();
                    shipment.Peso = (decimal)reader["Peso"];
                    shipment.DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"];
                    shipment.CostoSpedizione = (decimal)reader["CostoSpedizione"];
                    shipment.Nome = reader["Nome"].ToString();
                    shipment.Cognome = reader["Cognome"].ToString();

                    spedizioni.Add(shipment);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sqlConnection.Close();
            }

            return spedizioni;
        }
        public static int ContaInAttesa()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            int totalCount = 0;

            try
            {
                sqlConnection.Open();
                SqlCommand countCommand = new SqlCommand(
                    "SELECT COUNT(*) " +
                    "FROM Spedizioni " +
                    "WHERE StatoSpedizione = 'In attesa'", sqlConnection);

                totalCount = (int)countCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sqlConnection.Close();
            }

            return totalCount;
        }

        public static List<string> RaggruppaPerCitta()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            List<string> result = new List<string>();

            try
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand(
                    "SELECT CittaDestinataria, COUNT(*) as NumeroTotaleSpedizioni " +
                    "FROM Spedizioni " +
                    "GROUP BY CittaDestinataria", sqlConnection);

                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    string cittaDestinataria = reader["CittaDestinataria"].ToString();
                    int numeroTotaleSpedizioni = (int)reader["NumeroTotaleSpedizioni"];
                    result.Add($"{cittaDestinataria}: {numeroTotaleSpedizioni}");
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sqlConnection.Close();
            }

            return result;
        }
    }
}




    