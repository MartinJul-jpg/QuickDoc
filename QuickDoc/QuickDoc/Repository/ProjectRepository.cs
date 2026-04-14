using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Repository
{
    public class ProjectRepository
    {
        private Project project;
        private readonly string ConnectionString;

        public ProjectRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        public Project GetProject()
        {
            return project;
        }

        public void readFromDatabase(int projectNum, UnitRepository unitRepo)
        {
            
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT ProjectNumber, ProjectDescription FROM PROJECT WHERE ProjectNumber = @projectNum", con);
                cmd.Parameters.AddWithValue("@projectNum", projectNum);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int ProjectNum = (int)dr["ProjectNumber"];
                        string description = (string)dr["ProjectDescription"];

                        Project project = new Project(ProjectNum, description);
                        project.Units = unitRepo.GetAllUnits();
                        this.project = project;
                    }
                }
            }
        }
    }
}

