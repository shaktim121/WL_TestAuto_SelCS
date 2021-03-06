﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WL.TestAuto
{
    public class HRPage : AutomationCore
    {
        private IWebDriver driver;


        #region HR Page Object Collection
        [FindsBy(How = How.XPath, Using = "*//span[text()='Human Resources' or text()='Ressources humaines']")]
        private IWebElement Menu_HR { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"ctl00_MainMenu\"]/ul/li[2]/div")]
        private IWebElement Menu_SlideHR { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()=\"Employee\" or text()=\"Employé\"]")]
        private IWebElement Link_Employee { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()=\"Add Wizard\" or text()='Embauche']")]
        private IWebElement Link_AddWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type=\"button\" and @value=\"Search\"]")]
        private IWebElement Btn_SearchEmployee { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@class=\"rgMasterTable\" and contains(@id,\"ctl00_MainContent_EmployeeSearchControl1\")]")]
        private IWebElement Tbl_Employee { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and @id='ctl00_MainContent_WizardSearchControl1_EmployeeSummaryGrid_ctl00']")]
        private IWebElement Tbl_WizardEmployee { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='View Report']")]
        private IWebElement Btn_ViewReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@id,'AnniversaryListing') and contains(@src, 'EmployeeAnniversaryListing')]")]
        private IWebElement PDFReportAreaAL { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@id,'EmployeeListing') and contains(@src, 'EmployeeListing')]")]
        private IWebElement PDFReportAreaEL { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@id,'_pdfPane') and contains(@src, 'PDF')]")]
        private IWebElement PDFReportArea { get; set; }

        [FindsBy(How = How.XPath, Using = ".//embed[@type='application/pdf']")]
        private IWebElement PDFReportFile { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='PDF']")]
        private IWebElement Btn_ExportPDF { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='Excel']")]
        private IWebElement Btn_ExportExcel { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Employee Number']/parent::*/parent::*//input")]
        private IWebElement Txt_EmpNumber { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='First Name']/parent::*/parent::*//input")]
        private IWebElement Txt_FirstName { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Last Name']/parent::*/parent::*//input")]
        private IWebElement Txt_LastName { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='SIN']/parent::*/parent::*//input")]
        private IWebElement Txt_SIN { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Organizational Unit' or text()='Organization Unit']/parent::*/parent::*//input")]
        private IWebElement Txt_OrgUnit { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Add Wizard']")]
        private IWebElement Lbl_AddWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Add']")]
        private IWebElement Btn_AddToTable { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Insert']")]
        private IWebElement Btn_InsertToTable { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'EmployeeWindows')]")]
        private IWebElement Frame_EmployeeWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[contains(@id,'RadWindowWrapper_EmployeeWindows')]")]
        private IWebElement Win_AddEmployeeWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Biographical']")]
        private IWebElement Lbl_AddEmployeeBioWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Address']")]
        private IWebElement Lbl_AddEmployeeAddressWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Position']")]
        private IWebElement Lbl_AddEmployeePositionWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Phone']")]
        private IWebElement Lbl_AddEmployeePhoneWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Email']")]
        private IWebElement Lbl_AddEmployeeEmailWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Employment Information']")]
        private IWebElement Lbl_AddEmployeeInfoWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Statutory Deduction']")]
        private IWebElement Lbl_AddEmployeeStatutoryDedWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Banking Information']")]
        private IWebElement Lbl_AddEmployeeBankInfo { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Add Employee Wizard - Paycodes']")]
        private IWebElement Lbl_AddEmployeePaycodes { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type='submit' and @value='Cancel']")]
        private IWebElement Btn_CancelScreen { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[(@type='button' or @type='submit') and @value='Next']")]
        private IWebElement Btn_NextScreen { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[(@type='button' or @type='submit') and @value='Previous']")]
        private IWebElement Btn_PrevScreen { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type='submit' and @value='Save & Close']")]
        private IWebElement Btn_SaveClose { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[contains(@class,'RadComboBoxDropDown') and contains(@style,'display: block')]")]
        private IWebElement List_AllDrpDwn { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Title']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Title { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Gender']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Gender { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Marital Status']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_MaritalStatus { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Number']/parent::*/parent::*//input")]
        private IWebElement Txt_Number { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Birth Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_BirthDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Language']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Language { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Middle Name']/parent::*/parent::*//input")]
        private IWebElement Txt_MiddleName { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Known As Name']/parent::*/parent::*//input")]
        private IWebElement Txt_KnownName { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Identification No. Type']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_IdentificationNoType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Smoker']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Smoker { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[@id='ctl00_ContentPlaceHolder1_EmployeeControl1_EmployeeBiographicalControl_EmployeeView_EmploymentEquity_Field']")]
        private IWebElement ChkBoxList_EmpEquity { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Bilingualism']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Bilingualism { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Citizenship']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Citizenship { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Health Care Number']/parent::*/parent::*//input")]
        private IWebElement Txt_HealthCare { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Badge Number']/parent::*/parent::*//input")]
        private IWebElement Txt_Badge { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Type']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_AddressType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Address Line 1']/parent::*/parent::*//input")]
        private IWebElement Txt_Address1 { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Address Line 2']/parent::*/parent::*//input")]
        private IWebElement Txt_Address2 { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Postal/Zip Code']/parent::*/parent::*//input")]
        private IWebElement Txt_ZipCode { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='City']/parent::*/parent::*//input")]
        private IWebElement Txt_City { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Country']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Country { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Province']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Province { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type='button' and @value='Insert']")]
        private IWebElement Btn_Insert { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@class='rgMasterTable' and @id='ctl00_ContentPlaceHolder1_WizardAddEmployeeControl1_ctl00_PersonAddressGrid_ctl00']")]
        private IWebElement Tbl_PersonalAddress { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Effective Date']/parent::*/parent::*//input[contains(@class,'riTextBox riEnabled')]")]
        private IWebElement Txt_EffectiveDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Event/Action']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_EventAction { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Active']")]
        private IWebElement Txt_StatusActive { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Company']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Company { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Legal Entity']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_LegalEntity { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Business Unit']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_BusinessUnit { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Department']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Dept { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Job']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Job { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Position']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Position { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Regular/Temporary']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_RegTemp { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Employee Type']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_EmpType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Comp. Method' or text()='Compensation Method']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_CompMethod { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Standard Hours']/parent::*/parent::*//input")]
        private IWebElement Txt_StandardHours { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Comp. Amount']/parent::*/parent::*//input")]
        private IWebElement Txt_CompAmount { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[@id='ctl00_ContentPlaceHolder1_WizardAddEmployeeControl1_ctl00_EmployeePositionView_Workdays_Field']")]
        private IWebElement ChkBoxList_Workdays { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Process Group']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_ProcessGrp { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='WCB Code']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_WCBCode { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Hire Date']/parent::*/parent::*//input[@class='riTextBox riDisabled']")]
        private IWebElement Txt_HireDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Probation Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_ProbationDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Seniority Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_SeniorityDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Next Review Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_NextReviewDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Increase Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_IncreaseDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Anniversary Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_AnniversaryDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Business #']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Business { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[@id='ContentPlaceHolder1_WizardAddEmployeeControl1_ctl00_StatutoryDeductionView_FederalTaxClaim_Label' and text()='Tax Claim']/parent::*/parent::*//input")]
        private IWebElement Txt_FederalTaxClaim { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[@id='ContentPlaceHolder1_WizardAddEmployeeControl1_ctl00_StatutoryDeductionView_ProvincialTaxClaim_Label' and text()='Tax Claim']/parent::*/parent::*//input")]
        private IWebElement Txt_ProvTaxClaim { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Sequence']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Sequence { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Bank']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Bank { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Transit / Routing #']/parent::*/parent::*//input")]
        private IWebElement Txt_TransitRouting { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Account']/parent::*/parent::*//input")]
        private IWebElement Txt_Account { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Percentage']/parent::*/parent::*//input")]
        private IWebElement Txt_Percentage { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Account Type']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_AcType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@class='rgMasterTable' and @id='ctl00_ContentPlaceHolder1_WizardAddEmployeeControl1_ctl00_EmployeeBankingInfoGrid_ctl00']")]
        private IWebElement Tbl_BankInfo { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Paycode']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Paycode { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Amount/Rate']/parent::*/parent::*//input[contains(@id,'EmployeePaycodeIncomeView_AmountRate_Field') and @type='text']")]
        private IWebElement Txt_AmountRateIncome { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Amount/Rate']/parent::*/parent::*//input[contains(@id,'EmployeePaycodeDeductionView_AmountRate_Field') and @type='text']")]
        private IWebElement Txt_AmountRateDeduction { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Amount/Rate']/parent::*/parent::*//input[contains(@id,'EmployeePaycodeBenefitView_AmountRate_Field') and @type='text']")]
        private IWebElement Txt_AmountRateBenefit { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@id='ctl00_ContentPlaceHolder1_WizardAddEmployeeControl1_ctl00_EmployeePaycodeGrid_ctl00']")]
        private IWebElement Tbl_EmpPaycodes { get; set; }

        [FindsBy(How = How.XPath, Using = "*//ul/li/a/span[text()='Income']")]
        private IWebElement MenuOpt_Income { get; set; }

        [FindsBy(How = How.XPath, Using = "*//ul/li/a/span[text()='Deduction']")]
        private IWebElement MenuOpt_Deduction { get; set; }

        [FindsBy(How = How.XPath, Using = "*//ul/li/a/span[text()='Benefit']")]
        private IWebElement MenuOpt_Benefit { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@class='rwCloseButton' and @title='Close']")]
        private IWebElement Btn_CloseX { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Biographical']")]
        private IWebElement Btn_EmpBio { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[contains(text(),'Biographical')]")]
        private IWebElement Lbl_EmployeeBio { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[@class='rtsTxt' and text()='Personal']")]
        private IWebElement Tab_Personal { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[@class='rtsTxt' and text()='Address']")]
        private IWebElement Tab_Address { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@id='ctl00_ContentPlaceHolder1_EmployeeControl1_PersonPhoneControl_PersonContactChannelGrid_ctl00_ctl02_ctl00_EmployeeSummaryToolBar']//span[text()='Add']")]
        private IWebElement Btn_AddPhones { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[@class='rtsTxt' and text()='Phones']")]
        private IWebElement Tab_Phones { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@id='ctl00_ContentPlaceHolder1_EmployeeControl1_PersonPhoneControl_PersonContactChannelGrid_ctl00']")]
        private IWebElement Tbl_Phones { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Type']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_PhType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Phone Number']/parent::*/parent::*//input")]
        private IWebElement Txt_PhNumber { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[@class='rtsTxt' and text()='Email']")]
        private IWebElement Tab_Email { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@id='ctl00_ContentPlaceHolder1_EmployeeControl1_EmployeeBiographicalControl_EmployeeView_EmployeDetailToolBar']//span[text()='Edit']")]
        private IWebElement Btn_EditPersonal { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@id='ctl00_ContentPlaceHolder1_EmployeeControl1_EmployeeBiographicalControl_EmployeeView_EmployeDetailToolBar']//span[text()='Update']")]
        private IWebElement Btn_UpdatePersonal { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@id='ctl00_ContentPlaceHolder1_EmployeeControl1_EmployeeBiographicalControl_EmployeeView_EmployeDetailToolBar']//span[text()='Cancel']")]
        private IWebElement Btn_CancelPersonal { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@id='ctl00_ContentPlaceHolder1_EmployeeControl1_EmployeeAddressListControl_PersonAddressGrid_ctl00_ctl05_btnUpdate_input']")]
        private IWebElement Btn_UpdateAddress { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Terminations']")]
        private IWebElement Btn_EmpTerminations { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[contains(text(),'Termination')]")]
        private IWebElement Lbl_EmployeeTermination { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'PayrollWindows')]")]
        private IWebElement Frame_PayrollWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[@class='rtsTxt' and text()='Termination']")]
        private IWebElement Tab_Termination { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@id='ctl00_ContentPlaceHolder1_EmployeeTerminationPageControl1_EmployeeTerminationControl1_EmployeeTerminationView_EmployeeTerminationDetailToolBar']//span[text()='Edit']")]
        private IWebElement Btn_EditTermination { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Termination Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_TerminationDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Last Day Paid']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_LastDayPaid { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Event']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Event { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Reason']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Reason { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='ROE Reason']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_ROEReason { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@id='ctl00_ContentPlaceHolder1_EmployeeTerminationPageControl1_EmployeeTerminationControl1_EmployeeTerminationView_EmployeeDetailToolBar']//span[text()='Update']")]
        private IWebElement Btn_UpdateTermination { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@id='ctl00_ContentPlaceHolder1_EmployeeTerminationPageControl1_EmployeeTerminationControl1_EmployeeTerminationView_EmployeeDetailToolBar']//span[text()='Cancel']")]
        private IWebElement Btn_CancelTermination { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[text()='Status']/following::div[@class='RadListBox RadListBox_Silk RadListBoxScrollable']")]
        private IWebElement ChkBoxList_Status { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(text(),\"Server Error in '/' Application\")]")]
        private IWebElement Error_Server { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(text(),'An unhandled exception')]")]
        private IWebElement Error_UnhandledException { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(text(),'An error has occurred during report processing')]")]
        private IWebElement Error_ReportProcessing { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[text()='Year']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Year { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[text()='Paycode Type']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_PaycodeType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Start Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_StartDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@id='ctl00_MainContent_EmployeeDetailAuditReportControl1_StartDate_Field_popupButton']")]
        private IWebElement Calendar_EmpAuditStartDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='End Date']/following::input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_EndDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@id='ctl00_MainContent_EmployeeDetailAuditReportControl1_EndDate_Field_popupButton']")]
        private IWebElement Calendar_EmpAuditEndDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Skill']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Skill { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Organization Unit']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_OrgUnit { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_Date { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Month']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Month { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Contacts']")]
        private IWebElement Btn_Contacts { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[contains(text(),'Contacts')]")]
        private IWebElement Lbl_EmployeeContacts { get; set; }
        
        [FindsBy(How = How.XPath, Using = "*//em[contains(text(),'Emergency Contacts')]")]
        private IWebElement Lbl_EmergencyContacts { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[@class='rtsTxt' and text()='Emergency Contacts']")]
        private IWebElement Tab_EmergencyContacts { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[@id='ctl00_ContentPlaceHolder1_ContactModuleControl1_EmergencyContactTypeListControl_ContactTypeGrid']//span[text()='Edit']")]
        private IWebElement Btn_EditEmergencyContacts { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[text()='Update']")]
        private IWebElement Btn_Update { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[text()='Cancel']")]
        private IWebElement Btn_Cancel { get; set; }

        #endregion

        #region Constructor
        public HRPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Human Resources Page reusable Methods

        //Naviate to Screens in Human Resources
        public bool Fn_NavigateThroughHumanResources(string Option)
        {
            Boolean flag = false;
            try
            {
                if (Option.ToLower().Equals("employee"))
                {

                    //Code to verify Employee screen displayed
                    GenericMethods.SelectValueFromSlideDropDown(Menu_HR, "Human Resources", Menu_SlideHR, Option);
                    if (Link_Employee.Exists(30))
                    {
                        Link_Employee.Highlight();
                        test.Pass("Verified 'Human Resources -> Employee' on page");
                        test.Pass("Navigated to Employee Screen");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Human Resources -> Employee' on page");
                        test.Fail("Failed to navigate to Employee Screen");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("add wizard"))
                {
                    //code for add wizard
                    GenericMethods.SelectValueFromSlideDropDown(Menu_HR, "Human Resources", Menu_SlideHR, Option);
                    if (Link_AddWizard.Exists(30))
                    {
                        Link_AddWizard.Highlight();
                        test.Pass("Verified 'Human Resources -> Add Wizard' on page");
                        test.Pass("Navigated to Add Wizard Screen");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Human Resources -> Add Wizard' on page");
                        test.Fail("Failed to navigate to Add Wizard Screen");
                        flag = false;
                    }
                }

                if (Option.Split('-')[0].ToLower().Equals("reports"))
                {
                    //Code for Reports - FORMAT: 'Reports-Report Name'
                    GenericMethods.SelectValueFromSlideDropDown(Menu_HR, "Human Resources", Menu_SlideHR, Option.Split('-')[0], Option.Split('-')[1]);
                    IWebElement Link_subReport = driver.FindElement(By.XPath("*//a[text()='" + Option.Split('-')[1] + "']"));
                    if (Link_subReport.Exists(30))
                    {
                        Link_subReport.Highlight();
                        test.Pass("Verified 'Human Resources -> Reports -> " + Option.Split('-')[1] + "' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Human Resources -> Reports -> " + Option.Split('-')[1] + "' on page");
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Naviate to Screens in Human Resources - French
        public bool Fn_NavigateThroughRessourcesHumaines(string Option)
        {
            Boolean flag = false;
            try
            {
                if (Option.ToLower().Equals("employé"))
                {

                    //Code to verify Employee screen displayed
                    GenericMethods.SelectValueFromSlideDropDown(Menu_HR, "Ressources humaines", Menu_SlideHR, Option);
                    if (Link_Employee.Exists(30))
                    {
                        Link_Employee.Highlight();
                        test.Pass("Verified 'Ressources Humaines -> Employé' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Ressources Humaines -> Employé' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("embauche"))
                {
                    //code for add wizard
                    GenericMethods.SelectValueFromSlideDropDown(Menu_HR, "Ressources humaines", Menu_SlideHR, Option);
                    if (Link_AddWizard.Exists(30))
                    {
                        Link_AddWizard.Highlight();
                        test.Pass("Verified 'Ressources Humaines -> Embauche' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Ressources Humaines -> Embauche' on page");
                        flag = false;
                    }
                }

                if (Option.Split('-')[0].ToLower().Equals("reports"))
                {
                    //Code for Reports - FORMAT: 'Reports-Report Name'
                    GenericMethods.SelectValueFromSlideDropDown(Menu_HR, "Ressources humaines", Menu_SlideHR, Option.Split('-')[0], Option.Split('-')[1]);
                    IWebElement Link_subReport = driver.FindElement(By.XPath("*//a[text()='" + Option.Split('-')[1] + "']"));
                    if (Link_subReport.Exists(30))
                    {
                        Link_subReport.Highlight();
                        test.Pass("Verified 'Ressources Humaines -> Reports -> " + Option.Split('-')[1] + "' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Ressources Humaines -> Reports -> " + Option.Split('-')[1] + "' on page");
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Verify fields in Employee Screen with given parameters
        /// </summary>
        /// <param name="textFields"></param>
        /// <param name="buttons"></param>
        /// <param name="drpDowns"></param>
        /// <param name="checkBoxes"></param>
        /// <param name="toolBars"></param>
        /// <param name="tableColumns"></param>
        /// <returns></returns>
        public bool Fn_Verify_Fields_In_HR_Screens(string textFields, string buttons, string drpDowns, string checkBoxes, string toolBars, string tableColumns, string labelFields)
        {
            Boolean flag = false;
            try
            {
                //Code for text field verification
                if (textFields != null && textFields.Length > 0)
                {
                    string[] text = textFields.Split(';');
                    foreach (string t in text)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[@type=\"text\"]/parent::*/parent::*//span[text()=\"" + t + "\"]"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Text field: " + t + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find Text Field: " + t);
                            flag = false;
                        }
                    }
                }

                //Code to verify buttons
                if (buttons != null && buttons.Length > 0)
                {
                    string[] btn = buttons.Split(';');
                    foreach (string b in btn)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[(@type=\"button\" or @type='submit') and @value=\"" + b + "\"]"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Button: " + b + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find Button: " + b);
                            flag = false;
                        }
                    }
                }

                //Code to verify checkboxes
                if (checkBoxes != null && checkBoxes.Length > 0)
                {
                    string[] chk = checkBoxes.Split(';');
                    foreach (string ch in chk)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[@type=\"checkbox\"]/parent::*/span[text()=\"" + ch + "\"]"));

                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Checkbox: " + ch + " found");
                            flag = true;
                        }
                        else
                        {
                            IReadOnlyCollection<IWebElement> chkAll = driver.FindElements(By.XPath("*//input[@type='checkbox' and contains(@id,'" + ch + "')]"));

                            if (chkAll.Count > 0)
                            {
                                chkAll.FirstOrDefault().Highlight();
                                test.Pass("Checkbox: " + ch + " found");
                                flag = true;
                            }
                        }

                        if (!flag)
                        {
                            test.Fail("Failed to find Checkbox: " + ch);
                        }
                    }
                }

                //Code to verify Dropdown or Comboboxes
                if (drpDowns != null && drpDowns.Length > 0)
                {
                    string[] drp = drpDowns.Split(';');
                    foreach (string dd in drp)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//div[contains(@class,\"ComboBox\")]/parent::*/parent::*//span[text()=\"" + dd + "\"]"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("ComboBox: " + dd + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find ComboBox: " + dd);
                            flag = false;
                        }
                    }
                }

                //Code to verify Toolbar
                if (toolBars != null && toolBars.Length > 0)
                {
                    string[] toolBarL = toolBars.Split(';');
                    foreach (string tool in toolBarL)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//div[contains(@class,\"ToolBar\")]//span[text()=\"" + tool + "\"]"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("ToolBar option: " + tool + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find ToolBar option: " + tool);
                            flag = false;
                        }
                    }
                }

                //Code to verify Column Headers in Table
                if (tableColumns != null && tableColumns.Length > 0)
                {
                    string[] cols = tableColumns.Split(';');
                    foreach (string col in cols)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//th/a[text()=\"" + col + "\"]"));

                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Column name: " + col + " found");
                            flag = true;
                        }
                        else
                        {
                            IReadOnlyCollection<IWebElement> ele1 = driver.FindElements(By.XPath("*//th[text()=\"" + col + "\"]"));
                            if (ele1.Count > 0)
                            {
                                ele1.FirstOrDefault().Highlight();
                                test.Pass("Column name: " + col + " found");
                                flag = true;
                            }
                        }

                        if (!flag)
                        {
                            test.Fail("Failed to find Column name: " + col);
                        }
                    }
                }

                //Code to verify other Label fields
                if (labelFields != null && labelFields.Length > 0)
                {
                    string[] labels = labelFields.Split(';');
                    foreach (string label in labels)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//span[text()='" + label + "']"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Label name: " + label + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find Label name: " + label);
                            flag = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Search if Records should be displayed or not
        /// </summary>
        public bool Fn_Verify_Record_Displayed_In_EmployeeTable()
        {
            Boolean flag = false;
            try
            {
                if (Tbl_Employee.VerifyRecordDisplayedInTable(true))
                {
                    Btn_SearchEmployee.Click();

                    if (Tbl_Employee.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Employee records loaded");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Employee records not loaded");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("No records displayed should be visible");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Click on Button View Report and verify the PDF report loaded
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public bool Fn_Click_View_Report_And_Verify_PDF_Loaded(string reportName)
        {
            bool flag = false;
            try
            {
                if (Btn_ViewReport.Exists(10))
                {
                    Btn_ViewReport.Click();
                    if (PDFReportArea.Exists(120))
                    {
                        driver.SwitchTo().Frame(PDFReportArea);
                        if (PDFReportFile.Exists(60))
                        {
                            test.Pass("Report loaded successfully: " + reportName);
                            driver.SwitchTo().DefaultContent();
                            Thread.Sleep(3000);
                            flag = true;
                        }
                        else if (Error_Server.Exists(10) || Error_UnhandledException.Exists(10) || Error_ReportProcessing.Exists(10))
                        {
                            driver.SwitchTo().DefaultContent();
                            test.Fail("Error or Exception occured in Report page: " + reportName);
                            GenericMethods.CaptureScreenshot();
                        }
                        else
                        {
                            driver.SwitchTo().DefaultContent();
                            test.Fail("Failed to load or verify report: " + reportName);
                            GenericMethods.CaptureScreenshot();
                        }
                    }
                    else
                    {
                        driver.SwitchTo().DefaultContent();
                        test.Fail("Failed to load or verify report: " + reportName);
                        GenericMethods.CaptureScreenshot();
                    }
                }
                else
                {
                    test.Fail("Button View Report does not exist");
                    GenericMethods.CaptureScreenshot();
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Open and verify if report is displayed on screen
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public bool Fn_View_Verify_HR_ReportDisplayedOnScreen(string reportName)
        {
            Boolean flag = false;
            try
            {

                switch (reportName.ToLower())
                {
                    case "paycode report":
                        if (DrpDwn_Year.Exists(10) && DrpDwn_PaycodeType.Exists(10))
                        {
                            Btn_ExportExcel.Click();
                            test.Pass("Report page verified successfully: " + reportName);
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to load or verify report: " + reportName);
                            GenericMethods.CaptureScreenshot();
                        }
                        break;

                    case "employee detail audit report":
                        if (Txt_StartDate.Exists() && Txt_EndDate.Exists() && Txt_OrgUnit.Exists() && Txt_EmpNumber.Exists() && DrpDwn_CompMethod.Exists())
                        {
                            //Txt_StartDate.SetText(DateTime.Now.ToString("M/d/yyyy"));
                            Calendar_EmpAuditStartDate.SelectDateFromCalendarPopup(DateTime.Now.ToString("M/d/yyyy"));
                            Thread.Sleep(TimeSpan.FromSeconds(2));
                            //Txt_EndDate.Click();
                            //Thread.Sleep(TimeSpan.FromSeconds(2));
                            //Txt_EndDate.SetText(DateTime.Now.AddDays(1).ToString("M/d/yyyy"));
                            Calendar_EmpAuditEndDate.SelectDateFromCalendarPopup(DateTime.Now.AddDays(1).ToString("M/d/yyyy"));
                            Thread.Sleep(TimeSpan.FromSeconds(1));

                            flag = Fn_Click_View_Report_And_Verify_PDF_Loaded(reportName);
                            if (flag) Btn_ExportExcel.Click();

                        }
                        else
                        {
                            test.Fail("Failed to load or verify report: " + reportName);
                            GenericMethods.CaptureScreenshot();
                        }
                        break;

                    case "hr changes audit report":
                        if (Txt_StartDate.Exists(10) && Txt_EndDate.Exists(10) && Txt_OrgUnit.Exists(10))
                        {
                            Txt_StartDate.SetText(DateTime.Now.ToString("M/d/yyyy"));
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            Txt_EndDate.SetText(DateTime.Now.ToString("M/d/yyyy"));
                            Thread.Sleep(TimeSpan.FromSeconds(1));

                            flag = Fn_Click_View_Report_And_Verify_PDF_Loaded(reportName);
                            if (flag) Btn_ExportPDF.Click();
                        }
                        break;

                    case "skill report":
                        if (DrpDwn_Skill.Exists(10) && DrpDwn_OrgUnit.Exists(10))
                        {
                            flag = Fn_Click_View_Report_And_Verify_PDF_Loaded(reportName);
                            if (flag) Btn_ExportPDF.Click();
                        }
                        break;

                    case "stat holiday":
                        if (Txt_Date.Exists(10))
                        {
                            Txt_Date.SetText(DateTime.Now.ToString("M/d/yyyy"));
                            Thread.Sleep(TimeSpan.FromSeconds(1));

                            flag = Fn_Click_View_Report_And_Verify_PDF_Loaded(reportName);
                            if (flag) Btn_ExportPDF.Click();
                        }
                        break;

                    case "attrition report":
                        if (DrpDwn_Month.Exists(10) && DrpDwn_Year.Exists(10))
                        {
                            DrpDwn_Month.SelectValueFromDropDown("01 - January");
                            Thread.Sleep(TimeSpan.FromSeconds(2));

                            flag = Fn_Click_View_Report_And_Verify_PDF_Loaded(reportName);
                            if (flag) Btn_ExportPDF.Click();
                        }
                        break;

                    default:
                        flag = Fn_Click_View_Report_And_Verify_PDF_Loaded(reportName);
                        if (flag) Btn_ExportPDF.Click();
                        break;
                }

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Select from Status checkbox list
        public bool Fn_Select_Employee_Search_Status_CheckBox(string statusList)
        {
            bool flag = true;
            try
            {
                if (ChkBoxList_Status.Exists(10))
                {
                    foreach (string st in statusList.Split(';'))
                    {
                        if (ChkBoxList_Status.FindElements(By.XPath(".//span[text()='" + st + "']//preceding-sibling::input")).Count > 0)
                        {
                            //string chkStatus = ChkBoxList_Status.FindElement(By.XPath(".//span[text()='" + st + "']//preceding-sibling::input")).GetAttribute("checked").ToString();
                            if (ChkBoxList_Status.FindElements(By.XPath(".//span[text()='" + st + "']//preceding-sibling::input[@checked]")).Count <= 0)
                            {
                                ChkBoxList_Status.FindElement(By.XPath(".//span[text()='" + st + "']//preceding-sibling::input")).Click();
                            }

                        }
                        else
                        {
                            test.Fail("Failed to select check box: " + st);
                            flag = false;
                        }
                    }
                }
                else
                {
                    test.Fail("Failed to find Status check box list");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Search Employee with status
        public Dictionary<string, string> Fn_Search_Employees_With_Status(string statusList)
        {
            //bool flag = false;
            Dictionary<string, string> empName = new Dictionary<string, string>();
            try
            {
                if (Fn_Select_Employee_Search_Status_CheckBox(statusList))
                {
                    Btn_SearchEmployee.Click();
                    if (Tbl_Employee.VerifyRecordDisplayedInTable(false))
                    {
                        Tbl_Employee.FindElement(By.XPath("./tbody/tr[1]")).Click();
                        test.Pass("Employee searched with Status: " + statusList);

                        empName.Add("LastName", Tbl_Employee.FindElement(By.XPath("./tbody/tr[1]/td[2]")).Text);
                        empName.Add("FirstName", Tbl_Employee.FindElement(By.XPath("./tbody/tr[1]/td[3]")).Text);
                        //flag = true;
                    }
                    else
                    {
                        test.Fail("No Record present in Employee table");
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return empName;
        }

        //Search Employee Exist
        public bool Fn_Search_Employee_Exists(string empNum, string firstName, string lastName, string SIN, bool isExist)
        {
            Boolean flag = false;
            try
            {
                if (empNum != "")
                {
                    Txt_EmpNumber.SetText(empNum);
                }

                if (firstName != "")
                {
                    Txt_FirstName.SetText(firstName);
                }

                if (lastName != "")
                {
                    Txt_LastName.SetText(lastName);
                }

                if (SIN != "")
                {
                    Txt_SIN.SetText(SIN);
                }

                Btn_SearchEmployee.Click();
                Thread.Sleep(5000);

                if (!isExist)
                {
                    if (Tbl_Employee.VerifyRecordDisplayedInTable(true))
                    {
                        test.Pass("Employee record not found - As expected");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Employee record found - Not as expected");
                        flag = false;
                    }
                }
                else
                {
                    if (Tbl_Employee.VerifyRecordDisplayedInTable(false))
                    {
                        if (Tbl_Employee.FindElements(By.XPath("./tbody/tr[1]/td[text()='" + empNum + "']")).Count > 0)
                        {
                            Tbl_Employee.FindElement(By.XPath("./tbody/tr[1]")).Click();
                            test.Pass("Employee record Found : " + empNum);
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Employee record not found with Search criteria");
                            flag = false;
                        }
                    }
                    else
                    {
                        test.Fail("Employee record not found with Search criteria");
                        flag = false;
                    }
                }


            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Fuction to hire an employee
        public bool Fn_Hire_Employee(string empNumber, string empFName, string empLName, string empSIN, string processGrp)
        {
            Boolean flag = false;
            try
            {
                if (Lbl_AddWizard.Exists(30))
                {
                    Thread.Sleep(3000);
                    Btn_AddToTable.Click();

                    #region Select Process Group
                    driver.SwitchTo().Frame(Frame_EmployeeWizard);
                    //Code to select Process group might go in here if needed
                    Thread.Sleep(3000);
                    if (Btn_NextScreen.Exists(30))
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        Btn_NextScreen.Click();
                    }

                    driver.SwitchTo().DefaultContent();

                    #endregion

                    #region  Add Employee Wizard - Biographical
                    //Wait for Add Employee Wizard - Biographical
                    try
                    {
                        if (Lbl_AddEmployeeBioWizard.Exists(30))
                        {
                            Thread.Sleep(2000);
                            driver.SwitchTo().Frame(Frame_EmployeeWizard);

                            Txt_Number.SendKeys(empNumber);
                            Thread.Sleep(1000);
                            DrpDwn_Title.SelectValueFromDropDown("Mr.");
                            Thread.Sleep(1000);
                            Txt_FirstName.SendKeys(empFName);
                            Txt_LastName.SendKeys(empLName);
                            Thread.Sleep(1000);
                            DrpDwn_Gender.SelectValueFromDropDown("Male");
                            Thread.Sleep(1000);
                            Txt_SIN.Click();
                            Txt_SIN.SendKeys(Keys.Home);
                            Thread.Sleep(5000);
                            Txt_SIN.SendKeys(empSIN);
                            Thread.Sleep(3000);
                            Txt_BirthDate.SendKeys("01/01/1985");
                            Thread.Sleep(1000);
                            DrpDwn_MaritalStatus.SelectValueFromDropDown("Single");
                            Thread.Sleep(1000);

                            Btn_NextScreen.Click();
                            driver.SwitchTo().DefaultContent();

                        }
                        else
                        {
                            flag = false;
                            test.Fail("Process Group selection - Failed");
                            GenericMethods.CaptureScreenshot();
                        }
                    }
                    catch (Exception)
                    {
                        test.Error("Failed to enter details in 'Add Employee Wizard - Biographical' screen");
                        Assert.Fail("Failed to enter details in 'Add Employee Wizard - Biographical' screen");
                    }

                    #endregion


                    #region Add Employee Wizard - Address
                    //Wait for Add Employee Wizard - Address
                    try
                    {
                        if (Lbl_AddEmployeeAddressWizard.Exists(30))
                        {
                            flag = true;
                            test.Pass("Enter Employee Biographical - Passed");
                            Thread.Sleep(2000);
                            driver.SwitchTo().Frame(Frame_EmployeeWizard);

                            Txt_Address1.SendKeys("861 FETCHISON DRIVE");
                            Txt_Address2.SendKeys("OSHAWA ON");
                            Txt_ZipCode.SendKeys("L1K 0L6");
                            Txt_City.SendKeys("Ontario");
                            Thread.Sleep(1000);
                            DrpDwn_Country.SelectValueFromDropDown("Canada");
                            Thread.Sleep(7000);
                            DrpDwn_Province.SelectValueFromDropDown("Ontario");
                            Thread.Sleep(5000);

                            Btn_Insert.Click();
                            Thread.Sleep(5000);

                            if (Tbl_PersonalAddress.Exists(20))
                            {
                                Thread.Sleep(1000);
                                if (Tbl_PersonalAddress.FindElements(By.XPath("./tbody/tr/td[contains(text(),'861 FETCHISON DRIVE')]")).Count > 0)
                                {
                                    test.Pass("Address got added successfully");
                                    flag = true;
                                }
                                else
                                {
                                    test.Fail("Failed to add address");
                                    flag = false;
                                }
                            }
                        }
                        else
                        {
                            flag = false;
                            test.Fail("Enter Employee Biographical - Failed");
                            GenericMethods.CaptureScreenshot();
                        }
                    }
                    catch (Exception ex)
                    {
                        test.Error("Failed to enter details in 'Add Employee Wizard - Address' screen");
                        throw ex;
                    }

                    #endregion
                    Thread.Sleep(3000);
                    Btn_NextScreen.Click();
                    driver.SwitchTo().DefaultContent();

                    #region Add Employee Wizard - Phone
                    //Wait for Add Employee Wizard - Phone
                    if (Lbl_AddEmployeePhoneWizard.Exists(30))
                    {
                        driver.SwitchTo().Frame(Frame_EmployeeWizard);
                        Btn_NextScreen.Click();
                        driver.SwitchTo().DefaultContent();
                    }
                    #endregion

                    #region Add Employee Wizard - Email
                    //Wait for Add Employee Wizard - Email
                    if (Lbl_AddEmployeeEmailWizard.Exists(30))
                    {
                        driver.SwitchTo().Frame(Frame_EmployeeWizard);
                        Btn_NextScreen.Click();
                        driver.SwitchTo().DefaultContent();
                    }
                    #endregion

                    #region Add Employee Wizard - Position
                    //Wait for Add Employee Wizard - Position
                    try
                    {
                        if (Lbl_AddEmployeePositionWizard.Exists(30))
                        {
                            flag = true;
                            test.Pass("Enter Employee Address - Passed");
                            driver.SwitchTo().Frame(Frame_EmployeeWizard);
                            Thread.Sleep(2000);
                            Txt_EffectiveDate.SetText("01/01/2019");
                            Thread.Sleep(2000);
                            if (DrpDwn_EventAction.GetAttribute("value") == "New Hire")
                            {
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Even/Action not as expected");
                                flag = false;
                                GenericMethods.CaptureScreenshot();
                            }
                            //Getting Focus on the next dropdown
                            Txt_EffectiveDate.SendKeys(Keys.Tab);
                            Thread.Sleep(2000);
                            Txt_StatusActive.SendKeys(Keys.Tab);
                            Thread.Sleep(5000);

                            //DrpDwn_Company.SelectValueFromDropDown("QA");
                            DrpDwn_Company.SelectValueFromDropDown();
                            Thread.Sleep(10000);
                            //DrpDwn_LegalEntity.SelectValueFromDropDown("0897-653");
                            DrpDwn_LegalEntity.SelectValueFromDropDown();
                            Thread.Sleep(10000);
                            //DrpDwn_BusinessUnit.SelectValueFromDropDown("0906-0906");
                            DrpDwn_BusinessUnit.SelectValueFromDropDown();
                            Thread.Sleep(10000);
                            //DrpDwn_Dept.SelectValueFromDropDown("111165-205341");
                            DrpDwn_Dept.SelectValueFromDropDown();
                            Thread.Sleep(10000);

                            if (DrpDwn_Job.Exists(10))
                            {
                                //DrpDwn_Job.SelectValueFromDropDown("Job1");
                                DrpDwn_Job.SelectValueFromDropDown();
                                Thread.Sleep(10000);
                            }
                            if (DrpDwn_Position.Exists(10))
                            {
                                //DrpDwn_Position.SelectValueFromDropDown("Site Supervisor");
                                DrpDwn_Position.SelectValueFromDropDown();
                                Thread.Sleep(10000);
                            }

                            DrpDwn_RegTemp.SelectValueFromDropDown("Regular");
                            Thread.Sleep(10000);
                            DrpDwn_EmpType.SelectValueFromDropDown("Permanent");
                            Thread.Sleep(10000);
                            DrpDwn_CompMethod.SelectValueFromDropDown("Salaried");
                            Thread.Sleep(10000);
                            //Txt_StandardHours.Click();
                            GenericMethods.ClearText(Txt_StandardHours);
                            Txt_StandardHours.SendKeys("40");
                            GenericMethods.ClearText(Txt_CompAmount);
                            Txt_CompAmount.SendKeys("2500");
                            Thread.Sleep(2000);
                            if (ChkBoxList_Workdays.SelectCheckBoxFromList("Mon;Tues;Wed;Thur;Fri", true))
                            {
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Failed to check values in the list");
                                flag = false;
                                GenericMethods.CaptureScreenshot();
                            }
                        }
                        else
                        {
                            flag = false;
                            test.Fail("Enter Employee Address - Failed");
                            GenericMethods.CaptureScreenshot();
                        }
                    }
                    catch (Exception ex)
                    {
                        test.Error("Failed to enter details in 'Add Employee Wizard - Position' screen");
                        throw ex;
                    }
                    #endregion

                    Btn_NextScreen.Click();
                    driver.SwitchTo().DefaultContent();

                    #region Add Employee Wizard - Employement Info
                    //Wait for Add Employee Wizard - Employement Info
                    try
                    {

                        if (Lbl_AddEmployeeInfoWizard.Exists(30))
                        {
                            flag = true;
                            test.Pass("Enter Employee Position - Passed");
                            driver.SwitchTo().Frame(Frame_EmployeeWizard);

                            DrpDwn_ProcessGrp.SelectValueFromDropDown(processGrp);
                            Thread.Sleep(1000);
                            DrpDwn_WCBCode.SelectValueFromDropDown("Ontario WSIB");
                            Thread.Sleep(1000);
                            if (Txt_HireDate.GetAttribute("value") == "1/1/2019")
                            {
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Hire Date not as expected");
                                flag = false;
                                GenericMethods.CaptureScreenshot();
                            }
                            Txt_ProbationDate.Clear();
                            Txt_ProbationDate.SendKeys("4/1/2019");
                            Txt_SeniorityDate.Clear();
                            Txt_SeniorityDate.SendKeys("1/1/2020");
                            Txt_NextReviewDate.Clear();
                            Txt_NextReviewDate.SendKeys("7/1/2019");
                            Txt_IncreaseDate.Clear();
                            Txt_IncreaseDate.SendKeys("1/1/2020");
                            Txt_AnniversaryDate.Clear();
                            Txt_AnniversaryDate.SendKeys("1/1/2020");

                        }
                        else
                        {
                            flag = false;
                            test.Fail("Enter Employee Position - Failed");
                            GenericMethods.CaptureScreenshot();
                        }
                    }
                    catch (Exception ex)
                    {
                        test.Error("Failed to enter details in 'Add Employee Wizard - Employement Info' screen");
                        throw ex;
                    }
                    #endregion

                    Btn_NextScreen.Click();
                    driver.SwitchTo().DefaultContent();

                    #region Add Employee Wizard - Statutory Deduction
                    //Wait for Add Employee Wizard - Statutory Deduction
                    try
                    {

                        if (Lbl_AddEmployeeStatutoryDedWizard.Exists(30))
                        {
                            flag = true;
                            test.Pass("Enter Employement Info - Passed");
                            driver.SwitchTo().Frame(Frame_EmployeeWizard);

                            DrpDwn_Province.SelectValueFromDropDown("Ontario");
                            Thread.Sleep(5000);
                            //DrpDwn_Business.SelectValueFromDropDown("BusNum:1/1.17800");
                            DrpDwn_Business.SelectValueFromDropDown();
                            Thread.Sleep(3000);
                            if (Convert.ToInt32(Txt_FederalTaxClaim.GetAttribute("value").Replace(",", "")) > 0)
                            {
                                test.Pass("Federal Tax Claim displayed correctly : " + Txt_FederalTaxClaim.GetAttribute("value").ToString());
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Federal Tax Claim displayed incorrectly : " + Txt_FederalTaxClaim.GetAttribute("value").ToString());
                                flag = false;
                                GenericMethods.CaptureScreenshot();
                            }

                            if (Convert.ToInt32(Txt_ProvTaxClaim.GetAttribute("value").Replace(",", "")) > 0)
                            {
                                test.Pass("Provincial Tax Claim displayed correctly : " + Txt_ProvTaxClaim.GetAttribute("value").ToString());
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Provincial Tax Claim displayed incorrectly : " + Txt_ProvTaxClaim.GetAttribute("value").ToString());
                                flag = false;
                                GenericMethods.CaptureScreenshot();
                            }

                        }
                        else
                        {
                            flag = false;
                            test.Fail("Enter Employement Info - Failed");
                            GenericMethods.CaptureScreenshot();
                        }
                    }
                    catch (Exception ex)
                    {
                        test.Error("Failed to enter details in 'Add Employee Wizard - Statutory Deduction' screen");
                        throw ex;
                    }
                    #endregion

                    Btn_NextScreen.Click();
                    driver.SwitchTo().DefaultContent();

                    #region Add Employee Wizard - Bank Info
                    //Wait for Add Employee Wizard - Bank Info
                    try
                    {

                        if (Lbl_AddEmployeeBankInfo.Exists(30))
                        {
                            flag = true;
                            test.Pass("Enter Statutory Deduction - Passed");
                            driver.SwitchTo().Frame(Frame_EmployeeWizard);

                            Btn_AddToTable.Click();
                            Thread.Sleep(5000);
                            if (DrpDwn_Sequence.GetAttribute("value") == "NET")
                            {
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Sequence is not as expected");
                                GenericMethods.CaptureScreenshot();
                                flag = false;
                            }
                            DrpDwn_Country.SelectValueFromDropDown("Canada");
                            Thread.Sleep(5000);
                            //DrpDwn_Bank.SelectValueFromDropDown("001 - Bank of Montreal");
                            DrpDwn_Bank.SelectValueFromDropDown();
                            Thread.Sleep(5000);
                            Txt_TransitRouting.SendKeys("99999");
                            Txt_Account.SendKeys("666777888999");
                            if (Txt_Percentage.GetAttribute("value").ToString() == "100")
                            {
                                flag = true;
                            }
                            else
                            {
                                GenericMethods.CaptureScreenshot();
                                flag = false;
                            }
                            DrpDwn_AcType.SelectValueFromDropDown("Savings");
                            Thread.Sleep(5000);

                            Btn_Insert.Click();
                            Thread.Sleep(5000);

                            if (Tbl_BankInfo.Exists(20))
                            {
                                Thread.Sleep(1000);
                                if (Tbl_BankInfo.FindElements(By.XPath("./tbody/tr/td[contains(text(),'666777888999')]")).Count > 0)
                                {
                                    test.Pass("Bank Info got added successfully");
                                    flag = true;
                                }
                                else
                                {
                                    test.Fail("Failed to add Bank Info");
                                    GenericMethods.CaptureScreenshot();
                                    flag = false;
                                }
                            }

                        }
                        else
                        {
                            flag = false;
                            test.Fail("Enter Statutory Deduction - Failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        test.Error("Failed to enter details in 'Add Employee Wizard - Bank Info' screen");
                        throw ex;
                    }
                    #endregion

                    Btn_NextScreen.Click();
                    driver.SwitchTo().DefaultContent();

                    #region Add Employee Wizard - Paycodes
                    //Wait for Add Employee Wizard - Paycodes
                    try
                    {

                        if (Lbl_AddEmployeePaycodes.Exists(30))
                        {
                            flag = true;
                            test.Pass("Enter Bank Info - Passed");
                            driver.SwitchTo().Frame(Frame_EmployeeWizard);

                            //Write code to make it generic
                            //Income
                            Btn_AddToTable.Click();
                            Thread.Sleep(5000);
                            if (MenuOpt_Income.Displayed)
                            {
                                MenuOpt_Income.Click();
                                Thread.Sleep(5000);
                            }
                            DrpDwn_Paycode.SelectValueFromDropDown("COMTRA - Commuter / Transportation");
                            Thread.Sleep(5000);
                            Txt_AmountRateIncome.SendKeys("100");
                            Thread.Sleep(2000);
                            Btn_InsertToTable.Click();
                            Thread.Sleep(15000);
                            if (Tbl_EmpPaycodes.FindElements(By.XPath("./tbody/tr/td[contains(text(),'COMTRA')]")).Count > 0)
                            {
                                test.Pass("Paycode got added successfully");
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Failed to add Paycode");
                                GenericMethods.CaptureScreenshot();
                                flag = false;
                            }

                            //Write code to make it generic
                            //Deduction
                            Btn_AddToTable.Click();
                            Thread.Sleep(5000);
                            if (MenuOpt_Deduction.Displayed)
                            {
                                MenuOpt_Deduction.Click();
                                Thread.Sleep(5000);
                            }
                            DrpDwn_Paycode.SelectValueFromDropDown("DONATE - Donations");
                            Thread.Sleep(5000);
                            Txt_AmountRateDeduction.SendKeys("50");
                            Thread.Sleep(2000);
                            Btn_InsertToTable.Click();
                            Thread.Sleep(15000);
                            if (Tbl_EmpPaycodes.FindElements(By.XPath("./tbody/tr/td[contains(text(),'DONATE')]")).Count > 0)
                            {
                                test.Pass("Paycode got added successfully");
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Failed to add Paycode");
                                GenericMethods.CaptureScreenshot();
                                flag = false;
                            }

                            //Write code to make it generic
                            //Benefit
                            Btn_AddToTable.Click();
                            Thread.Sleep(5000);
                            if (MenuOpt_Benefit.Displayed)
                            {
                                MenuOpt_Benefit.Click();
                                Thread.Sleep(5000);
                            }
                            DrpDwn_Paycode.SelectValueFromDropDown("CARALL - Car Allowance");
                            Thread.Sleep(5000);
                            Txt_AmountRateBenefit.SendKeys("200");
                            Thread.Sleep(2000);
                            Btn_InsertToTable.Click();
                            Thread.Sleep(20000);
                            if (Tbl_EmpPaycodes.FindElements(By.XPath("./tbody/tr/td[contains(text(),'CARALL')]")).Count > 0)
                            {
                                test.Pass("Paycode got added successfully");
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Failed to add Paycode");
                                GenericMethods.CaptureScreenshot();
                                flag = false;
                            }

                            Thread.Sleep(5000);
                            Btn_Insert.Click();
                        }
                        else
                        {
                            flag = false;
                            test.Fail("Enter Bank Info - Failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        test.Error("Failed to enter details in 'Add Employee Wizard - Paycodes' screen");
                        throw ex;
                    }
                    #endregion

                    Thread.Sleep(5000);
                    driver.SwitchTo().DefaultContent();
                }
            }
            catch (Exception ex)
            {
                driver.SwitchTo().DefaultContent();
                Btn_CloseX.Click();
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Function to Terminate an Employee
        public bool Fn_Terminate_Employee(string terminationDate, string lastPaid, string eventName, string reason, [Optional] string ROEreason)
        {
            Boolean flag = false;
            try
            {
                if (Btn_EmpTerminations.Exists(10))
                {
                    Btn_EmpTerminations.Click();
                    if (Lbl_EmployeeTermination.Exists(20))
                    {
                        flag = true;
                        test.Pass("Employee Termination page displayed");
                        driver.SwitchTo().Frame(Frame_PayrollWizard);

                        if (driver.FindElements(By.XPath(".//span[text()='Event']//following::*//span[text()='Termination']")).Count > 0)
                        {
                            test.Pass("Employee already terminated");
                            flag = true;
                        }
                        else if (Tab_Termination.Exists(10))
                        {
                            Tab_Termination.Click();
                            Thread.Sleep(2000);
                            if (Btn_EditTermination.Enabled)
                            {
                                Btn_EditTermination.Click();
                                Thread.Sleep(2000);
                            }

                            Txt_TerminationDate.SendKeys(terminationDate);
                            Thread.Sleep(2000);
                            Txt_LastDayPaid.SendKeys(lastPaid);
                            Thread.Sleep(2000);
                            DrpDwn_Event.SelectValueFromDropDown(eventName);
                            Thread.Sleep(2000);
                            DrpDwn_Reason.SelectValueFromDropDown(reason);
                            Thread.Sleep(2000);
                            if (ROEreason != null)
                            {
                                DrpDwn_ROEReason.SelectValueFromDropDown(ROEreason);
                                Thread.Sleep(2000);
                            }

                            if (Btn_UpdateTermination.Enabled)
                            {
                                Btn_UpdateTermination.Click();
                                Thread.Sleep(5000);
                            }
                        }

                        driver.SwitchTo().DefaultContent();
                        if (Btn_CloseX.Exists(10))
                        {
                            Btn_CloseX.Click();
                        }

                        Thread.Sleep(10000);
                    }
                    else
                    {
                        test.Fail("Failed to display Employee Termination page");
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify status of Employee Record
        public bool Fn_Verify_Status_Of_Employee(string empNum, string empStatus)
        {
            Boolean flag = false;
            try
            {
                if (Tbl_Employee.VerifyRecordDisplayedInTable(false))
                {
                    IReadOnlyList<IWebElement> trTags = Tbl_Employee.FindElements(By.TagName("tr"));
                    foreach (IWebElement tr in trTags)
                    {
                        if (tr.FindElements(By.XPath("./td[text()='" + empNum + "']")).Count > 0)
                        {
                            if (tr.FindElements(By.XPath("./td[text()='" + empStatus + "']")).Count > 0)
                            {
                                flag = true;
                                test.Pass("Employee Record : " + empNum + " found with status: " + empStatus);
                                break;
                            }
                        }
                    }

                    if (!flag)
                    {
                        test.Fail("Failed to find Employee record : " + empNum + " with status : " + empStatus);
                    }
                }
                else
                {
                    test.Fail("Employee record not found with Search criteria");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Edit Biographical_Personal
        public bool Fn_Edit_Biographical_Details_Of_Employee(string empNo, string title, string fName, string lName, string language, string birthDate, string maritalStatus, string mName, string knownName, string gender, string identificationType, string SIN, string smoker, string empEquity, string bilingual, string citizenship, string healthCareNum, string badgeNum, string addressType, string add1, string add2, string zipCode, string city, string country, string province, string phoneType, string phNumber)
        {
            Boolean flag = true;
            try
            {
                Btn_EmpBio.Click();
                if (Lbl_EmployeeBio.Exists(10))
                {
                    driver.SwitchTo().Frame(Frame_EmployeeWizard);

                    #region Edit Personal details
                    Tab_Personal.Click();
                    Thread.Sleep(2000);
                    Btn_EditPersonal.Click();
                    Thread.Sleep(5000);

                    if (Txt_Number.GetAttribute("value") == empNo)
                    {
                        test.Pass("Employee record opened for editing");
                    }
                    else
                    {
                        test.Fail("Failed to open the required Employee record");
                        driver.SwitchTo().DefaultContent();
                        Btn_CloseX.Click();
                        return false;
                    }

                    if (title != "")
                    {
                        DrpDwn_Title.SelectValueFromDropDown(title);
                    }
                    Thread.Sleep(2000);
                    if (fName != "")
                    {
                        Txt_FirstName.SetText(fName);
                    }
                    if (lName != "")
                    {
                        Txt_LastName.SetText(lName);
                    }
                    if (language != "")
                    {
                        DrpDwn_Language.SelectValueFromDropDown(language);
                    }
                    Thread.Sleep(2000);

                    if (maritalStatus != "")
                    {
                        DrpDwn_MaritalStatus.SelectValueFromDropDown(maritalStatus);
                    }
                    Thread.Sleep(2000);
                    if (mName != "")
                    {
                        Txt_MiddleName.SetText(mName);
                    }
                    if (knownName != "")
                    {
                        Txt_KnownName.SetText(knownName);
                    }
                    if (gender != "")
                    {
                        DrpDwn_Gender.SelectValueFromDropDown(gender);
                    }
                    Thread.Sleep(2000);
                    if (identificationType != "")
                    {
                        DrpDwn_IdentificationNoType.SelectValueFromDropDown(identificationType);
                    }
                    Thread.Sleep(2000);
                    if (SIN != "")
                    {
                        Txt_SIN.Click();
                        Thread.Sleep(2000);
                        Txt_SIN.Clear();
                        Txt_SIN.SendKeys(Keys.Home);
                        Thread.Sleep(5000);
                        Txt_SIN.Click();
                        Txt_SIN.SendKeys(SIN);
                    }
                    Thread.Sleep(2000);
                    if (birthDate != "")
                    {
                        Txt_BirthDate.SetText(birthDate);
                    }
                    Thread.Sleep(2000);
                    if (smoker != "")
                    {
                        DrpDwn_Smoker.SelectValueFromDropDown(smoker);
                    }
                    Thread.Sleep(2000);
                    if (empEquity != "")
                    {
                        ChkBoxList_EmpEquity.SelectCheckBoxFromList(empEquity, true);
                    }
                    Thread.Sleep(2000);
                    if (bilingual != "")
                    {
                        DrpDwn_Bilingualism.SelectValueFromDropDown(bilingual);
                    }
                    Thread.Sleep(2000);
                    if (citizenship != "")
                    {
                        DrpDwn_Citizenship.SelectValueFromDropDown(citizenship);
                    }
                    Thread.Sleep(2000);
                    if (healthCareNum != "")
                    {
                        Txt_HealthCare.SetText(healthCareNum);
                    }
                    if (badgeNum != "")
                    {
                        Txt_Badge.SetText(badgeNum);
                    }
                    Thread.Sleep(5000);

                    if (Btn_UpdatePersonal.Exists(10))
                    {
                        Btn_UpdatePersonal.Click();
                        if (Btn_EditPersonal.Exists(10))
                        {
                            test.Pass("Edit Personal Detail Successful for the Record: " + empNo);
                        }
                        else
                        {
                            test.Fail("Failed to Edit and Save Personal Detail for Employee record: " + empNo);
                            return false;
                        }
                    }
                    #endregion

                    Thread.Sleep(3000);

                    #region Edit Address
                    Tab_Address.Click();
                    Thread.Sleep(4000);

                    if (driver.FindElements(By.XPath("*//table/tbody/tr[1]/td[text()='Home Address']")).Count > 0)
                    {
                        driver.FindElements(By.XPath("*//table/tbody/tr[1]/td/input[@type='image' and @title='Edit']"))[0].Click();
                        if (DrpDwn_AddressType.Exists(10))
                        {
                            test.Pass("Address Edit page opened for Employee");

                            if (addressType != "")
                            {
                                DrpDwn_AddressType.SelectValueFromDropDown(addressType);
                            }
                            Thread.Sleep(2000);
                            if (add1 != "")
                            {
                                Txt_Address1.SetText(add1);
                            }
                            if (add2 != "")
                            {
                                Txt_Address2.SetText(add2);
                            }
                            if (zipCode != "")
                            {
                                Txt_ZipCode.SetText(zipCode);
                            }
                            Thread.Sleep(2000);
                            if (city != "")
                            {
                                Txt_City.SetText(city);
                            }
                            if (country != "")
                            {
                                DrpDwn_Country.SelectValueFromDropDown(country);
                            }
                            Thread.Sleep(7000);
                            if (province != "")
                            {
                                DrpDwn_Province.SelectValueFromDropDown(province);
                            }
                            Thread.Sleep(7000);

                            if (Btn_UpdateAddress.Exists(5))
                            {
                                Btn_UpdateAddress.Click();
                                if (Btn_AddToTable.Exists(10))
                                {
                                    test.Pass("Edit Address Successful for the Record: " + empNo);
                                }
                                else
                                {
                                    test.Fail("Failed to Edit and Save Address for Employee record: " + empNo);
                                    return false;
                                }
                            }

                        }
                    }
                    #endregion

                    Thread.Sleep(3000);

                    #region Edit Phones
                    Tab_Phones.Click();
                    Thread.Sleep(4000);

                    if (Tbl_Phones.FindElements(By.XPath(".//*[text()='No records to display.']")).Count > 0)
                    {
                        Btn_AddPhones.Click();
                        DrpDwn_PhType.SelectValueFromDropDown(phoneType);
                        Thread.Sleep(3000);
                        Txt_PhNumber.Click();
                        Thread.Sleep(2000);
                        Txt_PhNumber.SendKeys(Keys.Home);
                        Thread.Sleep(2000);
                        Txt_PhNumber.SendKeys(phNumber);
                        Thread.Sleep(3000);
                        Btn_Insert.Click();
                        Thread.Sleep(5000);
                    }
                    #endregion

                    driver.SwitchTo().DefaultContent();
                    Btn_CloseX.Click();
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                if (Lbl_EmployeeBio.Exists(10))
                {
                    driver.SwitchTo().DefaultContent();
                    Btn_CloseX.Click();
                    Thread.Sleep(5000);
                }

                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify Emergency Contact screen
        public bool Fn_Verify_Employee_Emergency_Contact_Screen()
        {
            bool flag = false;
            try
            {
                if(Btn_Contacts.Exists(10))
                {
                    Btn_Contacts.Click();
                    if(Lbl_EmployeeContacts.Exists(20))
                    {
                        flag = true;
                        test.Pass("Employee Contacts page displayed");
                        driver.SwitchTo().Frame(Frame_EmployeeWizard);

                        if (Tab_EmergencyContacts.Exists(10)) Tab_EmergencyContacts.Click();
                        if (Btn_EditEmergencyContacts.Exists(10)) Btn_EditEmergencyContacts.Click();
                        driver.SwitchTo().Frame("EmployeeWindow");

                        if(Btn_Update.Exists(10) && Btn_Cancel.Exists(10))
                        {
                            flag = true;
                            test.Pass("Employee Emergency Contact screen verified successfully");
                        }
                        else
                        {
                            flag = false;
                            test.Fail("Failed to verify Employee Contacts screen");
                            GenericMethods.CaptureScreenshot();
                        }
                        driver.SwitchTo().ParentFrame();
                        Btn_CloseX.Click();
                        Thread.Sleep(2000);
                        driver.SwitchTo().DefaultContent();
                        Btn_CloseX.Click();
                    }
                    else
                    {
                        test.Fail("Failed to open Employee Contacts Window");
                        GenericMethods.CaptureScreenshot();
                    }
                }
                else
                {
                    test.Fail("Failed to Click Employee Contacts button");
                    GenericMethods.CaptureScreenshot();
                }
            }
            catch(Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }


        #endregion
    }

}
