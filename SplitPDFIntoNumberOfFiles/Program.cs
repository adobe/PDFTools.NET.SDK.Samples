﻿/*
 * Copyright 2019 Adobe
 * All Rights Reserved.
 *
 * NOTICE: Adobe permits you to use, modify, and distribute this file in 
 * accordance with the terms of the Adobe license agreement accompanying 
 * it. If you have received this file from a source other than Adobe, 
 * then your use, modification, or distribution of it requires the prior 
 * written permission of Adobe.
 */

using System.IO;
using System;
using System.Collections.Generic;
using log4net.Repository;
using log4net.Config;
using log4net;
using System.Reflection;
using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK.pdfops;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.exception;


namespace SplitPDFIntoNumberOfFiles
{
    /// <summary>
    /// This sample illustrates how to split input PDF into multiple PDF files on the basis of the number of documents.
    /// <para/>
    /// Refer to README.md for instructions on how to run the samples.
    /// </summary>
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        static void Main()
        {
            //Configure the logging
            ConfigureLogging();
            try
            {
                // Initial setup, create credentials instance.
                Credentials credentials = Credentials.ServiceAccountCredentialsBuilder()
                                .FromFile(Directory.GetCurrentDirectory() + "/pdfservices-api-credentials.json")
                                .Build();

                // Create an ExecutionContext using credentials.
                ExecutionContext executionContext = ExecutionContext.Create(credentials);

                // Create a new operation instance
                SplitPDFOperation splitPDFOperation = SplitPDFOperation.CreateNew();

                // Set operation input from a source file.
                FileRef sourceFileRef = FileRef.CreateFromLocalFile(@"splitPDFInput.pdf");
                splitPDFOperation.SetInput(sourceFileRef);

                // Set the number of documents to split the input PDF file into.
                splitPDFOperation.SetFileCount(2);

                // Execute the operation.
                List<FileRef> result = splitPDFOperation.Execute(executionContext);

                // Save the result to the specified location.
                int index = 0;
                foreach (FileRef fileRef in result)
                {
                    fileRef.SaveAs(Directory.GetCurrentDirectory() + "/output/SplitPDFIntoNumberOfFilesOutput_" + index + ".pdf");
                    index++;
                }

            }
            catch (ServiceUsageException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (ServiceApiException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (SDKException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (IOException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (Exception ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
        }

        static void ConfigureLogging()
        {
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
