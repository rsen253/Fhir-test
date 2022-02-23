using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;


namespace FHIR_Test_App
{
    public static class Program
    {
        private const string _fhirServer = "http://hapi.fhir.org/baseR4"; //http://hapi.fhir.org/baseR4
        static void Main(string[] args)
        {
            FhirClient fhirClient = new FhirClient(_fhirServer)
            {
                PreferredFormat = ResourceFormat.Json,
                PreferredReturn = Prefer.ReturnRepresentation
            };

            // get some patient form server
            var patientBundle = fhirClient.Search<Patient>(new string[] { "name=Mike" });

            Console.WriteLine($"Total: {patientBundle.Total} , Entry Count: {patientBundle.Entry.Count}");
            int count = 0;

            while (patientBundle != null)
            {
                foreach (Bundle.EntryComponent entry in patientBundle.Entry)
                {
                    if (entry.Resource != null)
                    {
                        Patient patient = (Patient)entry.Resource;
                        if (patient.Name.Count > 0)
                        {
                            System.Console.WriteLine($"Bundle: {patientBundle.Total} Entry:{count,3} Id: {patient.Id} Name: {patient.Name[0].ToString()}");
                        }

                    }

                    count++;
                }
                patientBundle = fhirClient.Continue(patientBundle);
            }

        }
    }
}
