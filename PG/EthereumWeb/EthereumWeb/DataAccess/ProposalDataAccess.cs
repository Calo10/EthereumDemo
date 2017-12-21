using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace EthereumWeb.Models
{
    public class ProposalDataAccess : DataAccess
    {
        public bool InsertOption(int idOption, string contracEthereumProposal, string description)
        {
            bool isValid = false;
            try
            {
                string query = "INSERT INTO `Option` (`IdOption`, `ContracEthereumProposal`, `Description`) VALUES (@IdOption, @ContracEthereumProposal, @Description); ";


                List<ParameterSchema> lstParams = new List<ParameterSchema>();

                lstParams.Add(new ParameterSchema("ContracEthereumProposal", contracEthereumProposal));
                lstParams.Add(new ParameterSchema("Description", description));
                lstParams.Add(new ParameterSchema("IdOption", idOption));

                ExecuteQuery(query, lstParams);


                isValid = true;
            }
            catch (Exception err)
            {

                isValid = false;
            }
            return isValid;
        }

        public bool DeleteProposal(ProposalModel proposal)
        {
            bool isValid = false;
            MySqlTransaction transaction = Connection.BeginTransaction();
            try
            {
                string query = "delete from ProposalUser where ContracEthereumProposal='" + proposal.ContracEthereumProposal + "'";
                ExecuteNonQuery(query);

                query = "delete from `Option` where ContracEthereumProposal='" + proposal.ContracEthereumProposal + "'";
                ExecuteNonQuery(query);

                query = "delete from Proposal where ContracEthereumProposal='" + proposal.ContracEthereumProposal + "'";
                ExecuteNonQuery(query);

                isValid = true;
                transaction.Commit();
            }
            catch (Exception err)
            {
                transaction.Rollback();
            }
            return isValid;
        }

        public bool InsertProposal(ProposalModel proposal)
        {
            bool isValid = false;
            MySqlTransaction transaction = Connection.BeginTransaction();
            try
            {

                List<ParameterSchema> lstParams = new List<ParameterSchema>();
                string query = string.Empty;

                lstParams.Add(new ParameterSchema("ContracEthereumProposal", proposal.ContracEthereumProposal));
                lstParams.Add(new ParameterSchema("ProposalName", proposal.ProposalName));
                lstParams.Add(new ParameterSchema("SecurityType", (int)proposal.SecurityType));
                lstParams.Add(new ParameterSchema("QuestionType", (int)proposal.QuestionType));
                lstParams.Add(new ParameterSchema("InitialDate", proposal.InitialDate));
                lstParams.Add(new ParameterSchema("FinalDate", proposal.FinalDate));
                lstParams.Add(new ParameterSchema("AdvancedSearch", proposal.AdvancedSearch));
                lstParams.Add(new ParameterSchema("Description", proposal.Description));
                lstParams.Add(new ParameterSchema("MinimumQuantitySelected", proposal.MinimumQuantitySelected));
                lstParams.Add(new ParameterSchema("MaximumQuantitySelected", proposal.MaximumQuantitySelected));
                lstParams.Add(new ParameterSchema("UserCreator", proposal.UserCreator));

                query = "INSERT INTO dbEthereumDemo.Proposal(ContracEthereumProposal,ProposalName,SecurityType,QuestionType,InitialDate,FinalDate,AdvancedSearch,Description,MinimumQuantitySelected, MaximumQuantitySelected, UserCreator )" +
                                "VALUES(@ContracEthereumProposal, @ProposalName, @SecurityType, @QuestionType, @InitialDate, @FinalDate, @AdvancedSearch, @Description, @MinimumQuantitySelected, @MaximumQuantitySelected, @UserCreator); ";

                ExecuteQuery(query, lstParams);

                if (InsertProposalUser(proposal.ContracEthereumProposal, proposal.UserCreator))
                {

                    foreach (OptionModel option in proposal.Options)
                    {
                        InsertOption(option.IdOption, proposal.ContracEthereumProposal, option.Description);
                    }

                }

                isValid = true;
                transaction.Commit();
            }
            catch (Exception err)
            {
                transaction.Rollback();
                isValid = false;
            }
            return isValid;
        }

        public bool InsertProposalUser(string ContracEthereumProposal, string email)
        {
            bool isValid = false;
            try
            {
                List<ParameterSchema> lstParams = new List<ParameterSchema>();
                string query = string.Empty;

                lstParams.Add(new ParameterSchema("ContracEthereumProposal", ContracEthereumProposal));
                lstParams.Add(new ParameterSchema("Email", email));

                query = "INSERT INTO dbEthereumDemo.ProposalUser(ContracEthereumProposal,Email)" +
                                "VALUES(@ContracEthereumProposal, @Email); ";

                ExecuteQuery(query, lstParams);

                isValid = true;
            }
            catch (Exception)
            {
                isValid = false;
            }
            return isValid;
        }

        public bool UpdateProposal(ProposalModel proposal)
        {
            bool isValid = true;

            try
            {
                List<ParameterSchema> lstParams = new List<ParameterSchema>();
                string query = string.Empty;

                lstParams.Add(new ParameterSchema("ContracEthereumProposal", proposal.ContracEthereumProposal));
                lstParams.Add(new ParameterSchema("ProposalName", proposal.ProposalName));
                lstParams.Add(new ParameterSchema("SecurityType", proposal.SecurityType));
                lstParams.Add(new ParameterSchema("InitialDate", proposal.InitialDate));
                lstParams.Add(new ParameterSchema("FinalDate", proposal.FinalDate));
                lstParams.Add(new ParameterSchema("Description", proposal.Description));
                lstParams.Add(new ParameterSchema("MinimumQuantitySelected", proposal.MinimumQuantitySelected));
                lstParams.Add(new ParameterSchema("MaximumQuantitySelected", proposal.MaximumQuantitySelected));

                query = "Update Proposal Set " +
                        "ProposalName=@ProposalName," +
                        "SecurityType=@SecurityType, " +
                        "InitialDate=@InitialDate, " +
                        "FinalDate=@FinalDate, " +
                        "InitialDate=@InitialDate, " +
                        "MinimumQuantitySelected=@MinimumQuantitySelected, " +
                        "MaximumQuantitySelected=@MaximumQuantitySelected  " +
                        "where ContracEthereumProposal=@ContracEthereumProposal";

                ExecuteQuery(query, lstParams);

                return isValid = true;
            }
            catch (Exception err)
            {
                isValid = false;
            }
            return isValid;
        }

        public List<ProposalModel> SearchProposalByUser(string email, EnumQuestionType? questionType)
        {
            try
            {
                string query = "SELECT p.ContracEthereumProposal," +
                                                "p.ProposalName," +
                                                "p.SecurityType," +
                                                "p.QuestionType," +
                                                "p.InitialDate," +
                                                "p.FinalDate," +
                                                "p.AdvancedSearch," +
                                                "p.ContracEthereumProposal," +
                                                "p.Description, " +
                                                "p.MinimumQuantitySelected," +
                                                "p.MaximumQuantitySelected, " +
                                                "p.UserCreator, " +
                                                "CASE WHEN p.UserCreator=pu.Email THEN 1 else 0 END 'IsMine', " +
                                                "pu.IsVoted " +
                        "FROM Proposal p INNER JOIN ProposalUser pu " +
                                "ON p.ContracEthereumProposal=pu.ContracEthereumProposal " +
                                 " INNER JOIN User u ON pu.Email=u.Email " +
                        "WHERE u.Email = '" + email + "' ";
                // "AND NOW() BETWEEN p.InitialDate AND p.FinalDate;";

                if (questionType != null)
                {
                    query += " AND QuestionType=" + questionType.ToString();
                }

                List<ProposalModel> proposals = new List<ProposalModel>();

                using (MySqlDataReader reader = ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        ProposalModel proposal = new ProposalModel();

                        proposal.ContracEthereumProposal = reader["ContracEthereumProposal"].ToString();
                        proposal.ProposalName = reader["ProposalName"].ToString();
                        proposal.SecurityType = (EnumSecurityType)Convert.ToInt32(reader["SecurityType"]);
                        proposal.QuestionType = (EnumQuestionType)Convert.ToInt32(reader["QuestionType"]);
                        proposal.InitialDate = Convert.ToDateTime(reader["InitialDate"]);
                        proposal.FinalDate = Convert.ToDateTime(reader["FinalDate"]);
                        proposal.AdvancedSearch = Convert.ToInt32(reader["AdvancedSearch"]);
                        proposal.Description = reader["Description"].ToString();
                        proposal.IsVoted = Convert.ToBoolean(reader["IsVoted"]);
                        proposal.MinimumQuantitySelected = Convert.ToInt32(reader["MinimumQuantitySelected"]);
                        proposal.MaximumQuantitySelected = Convert.ToInt32(reader["MaximumQuantitySelected"]);
                        proposal.UserCreator = reader["UserCreator"].ToString();
                        proposal.IsMine = Convert.ToBoolean(reader["IsMine"]);
                        proposal.Options = new List<OptionModel>();

                        using (DataAccess con = new DataAccess())
                        {
                            string quer = "SELECT IdOption, " +
                            "ContracEthereumProposal, " +
                            "Description " +
                            "FROM `Option` o " +
                            "WHERE o.ContracEthereumProposal='" + proposal.ContracEthereumProposal + "'";

                            using (MySqlDataReader r = con.ExecuteQuery(quer))
                            {
                                while (r.Read())
                                {
                                    OptionModel option = new OptionModel()
                                    {
                                        Description = r["Description"].ToString(),
                                        IdOption = Convert.ToInt32(r["IdOption"])
                                    };

                                    proposal.Options.Add(option);
                                }
                                r.Close();
                            }

                            proposals.Add(proposal);
                        }
                    }
                    reader.Close();
                }

                return proposals;
            }
            catch (Exception err)
            {

                throw err;
            }
        }

        public List<OptionModel> SearchOptionByProposal(string contracEthereumProposal)
        {
            try
            {
                string query = "SELECT IdOption, " +
                        "ContracEthereumProposal, " +
                        "Description " +
                        "FROM `Option` o " +
                        "WHERE o.ContracEthereumProposal='" + contracEthereumProposal + "'";

                List<OptionModel> options = new List<OptionModel>();
                MySqlDataReader reader = ExecuteQuery(query);

                while (reader.Read())
                {
                    OptionModel option = new OptionModel();

                    option.Description = reader["Description"].ToString();
                    option.IdOption = Convert.ToInt32(reader["IdOption"]);

                    options.Add(option);
                }

                return options;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public List<ProposalModel> SearchProposal(string ethereumContractUser, string ProposalName)
        {
            try
            {
                string query = "SELECT Proposal.ContracEthereumProposal," +
                                    "Proposal.ProposalName," +
                                    "Proposal.SecurityType," +
                                    "Proposal.QuestionType," +
                                    "Proposal.ProposalName," +
                                    "Proposal.InitialDate, " +
                                    "Proposal.FinalDate," +
                                    "Proposal.AdvancedSearch, " +
                                    "Proposal.Description, " +
                                    "Proposal.UserCreator," +
                                    "Proposal.MinimumQuantitySelected, " +
                                    "Proposal.MaximumQuantitySelected " +
                                    "FROM dbEthereumDemo.Proposal ";

                StringBuilder criteria = new StringBuilder();

                if (ethereumContractUser != null)
                {
                    if (criteria.ToString() == string.Empty)
                    {
                        query += " Where ";
                    }
                    else
                    {
                        criteria.Append(" AND ");
                    }
                    query += ("ContracEthereumProposal = '" + ethereumContractUser + "'");
                }

                if (ProposalName != null)
                {
                    if (criteria.ToString() == string.Empty)
                    {
                        criteria.Append(" Where ");
                    }
                    else
                    {
                        criteria.Append(" AND ");
                    }
                    criteria.Append("ProposalName='" + ProposalName + "'");
                }

                List<ProposalModel> proposals = new List<ProposalModel>();

                using (MySqlDataReader reader = ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        ProposalModel proposal = new ProposalModel();

                        proposal.ContracEthereumProposal = reader["ContracEthereumProposal"].ToString();
                        proposal.ProposalName = reader["ProposalName"].ToString();
                        proposal.SecurityType = (EnumSecurityType)Convert.ToInt32(reader["SecurityType"]);
                        proposal.QuestionType = (EnumQuestionType)Convert.ToInt32(reader["QuestionType"]);
                        proposal.InitialDate = Convert.ToDateTime(reader["InitialDate"]);
                        proposal.FinalDate = Convert.ToDateTime(reader["FinalDate"]);
                        proposal.AdvancedSearch = Convert.ToInt32(reader["AdvancedSearch"]);
                        proposal.Description = reader["Description"].ToString();
                        proposal.MinimumQuantitySelected = Convert.ToInt32(reader["MinimumQuantitySelected"]);
                        proposal.MaximumQuantitySelected = Convert.ToInt32(reader["MaximumQuantitySelected"]);
                        proposal.UserCreator = reader["UserCreator"].ToString();

                        proposal.Options = new List<OptionModel>();

                        using (DataAccess con = new DataAccess())
                        {
                            string quer = "SELECT IdOption, " +
                            "ContracEthereumProposal, " +
                            "Description " +
                            "FROM `Option` o " +
                            "WHERE o.ContracEthereumProposal='" + proposal.ContracEthereumProposal + "'";

                            using (MySqlDataReader r = con.ExecuteQuery(quer))
                            {
                                while (r.Read())
                                {
                                    OptionModel option = new OptionModel()
                                    {
                                        Description = r["Description"].ToString(),
                                        IdOption = Convert.ToInt32(r["IdOption"])
                                    };

                                    proposal.Options.Add(option);
                                }
                                r.Close();
                            }

                            proposals.Add(proposal);
                        }

                        proposals.Add(proposal);
                    }
                }

                return proposals;
            }
            catch (Exception err)
            {

                throw err;
            }
        }

        public bool UserVoted(string email, string contracEthereumProposal)
        {
            bool isValid = false;
            try
            {

                string query = "UPDATE ProposalUser SET IsVoted=1 " +
                                "WHERE ContracEthereumProposal='" + contracEthereumProposal + "' AND Email='" + email + "'";

                ExecuteQuery(query);

                isValid = true;
            }
            catch (Exception err)
            {
                isValid = false;
            }
            return isValid;
        }
    }
}