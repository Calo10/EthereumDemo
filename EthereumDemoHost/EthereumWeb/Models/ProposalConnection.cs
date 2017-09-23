using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class ProposalConnection
    {
        Connection conexionM = new Connection();

        public bool InserProposal(PoposalModel proposal)
        {
            bool isValid = true;

            try
            {

                List<ParameterSchema> lstParams = new List<ParameterSchema>();
                string query = string.Empty;

                lstParams.Add(new ParameterSchema("ContracEthereumProposal", proposal.ContracEthereumProposal));
                lstParams.Add(new ParameterSchema("ProposalName", proposal.ProposalName));
                lstParams.Add(new ParameterSchema("ProposalDescription", proposal.ProposalDescription));



                if (!string.IsNullOrEmpty(proposal.ContracEthereumProposal))
                {
                    //Inserta Nuevo Curso
                    query = "INSERT INTO dbEthereumDemo.Proposal(ContracEthereumProposal,ProposalName,ProposalDescription)" +
                            "VALUES(@ContracEthereumProposal, @NameProposal, @DescriptionProposal); ";


                }
                else
                {
                    query = "Update Curso Set Nombre= @Nombre,Descripcion = @Descripcion, Tipo_Curso= @Tipo_Curso, Nivel_Curso= @Nivel_Curso, PPT_Enbed= @PPT_Enbed, HTML_Code=@HTML_Code, Lineas_Codigo= @Lineas_Codigo, " +
                                   "Youtube_URL = @Youtube_URL, Repositorio_GH= @Repositorio_GH, Horas_Video= @Horas_Video, Privacidad = @Privacidad,Codigo_Acceso = @Codigo_Acceso, Estado = @Estado, Autor = @Autor  where id = @id";

                }


                return conexionM.setExecuteQuery(query, lstParams).Contains("ID");

            }
            catch (Exception err)
            {

                isValid = false;
            }
            return isValid;
        }

        public List<PoposalModel> SearchProposal(string ethereumContractUser, string ProposalName)
        {
            try
            {
                string query = "SELECT Proposal.ContracEthereumProposal,Proposal.ProposalName,Proposal.ProposalDescription FROM dbEthereumDemo.Proposal Where (ContracEthereumProposal ='" + ethereumContractUser + "'or '"+ ethereumContractUser + "' is null) and (AND ProposalName='" + ProposalName + "' or '"+ ProposalName + "'=null)";

               List< PoposalModel> proposals = new List<PoposalModel>();
                MySqlDataReader reader = conexionM.getExecuteQuery(query);

                while (reader.Read())
                {
                    PoposalModel proposal = new PoposalModel();

                    proposal.ContracEthereumProposal = reader["ContracEthereumProposal"].ToString();
                    proposal.ProposalName = reader["ProposalName"].ToString();
                    proposal.ProposalDescription = reader["ProposalDescription"].ToString();

                    proposals.Add(proposal);
                }

                return proposals;

            }
            catch (Exception err)
            {

                throw err;
            }
        }
    }
    public class PoposalModel{
        public string ContracEthereumProposal { get; set; }
        public string ProposalName { get; set; }
        public string ProposalDescription { get; set; }
    }

}