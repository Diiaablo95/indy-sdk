﻿using Indy.Sdk.Dotnet.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using static Indy.Sdk.Dotnet.Wrapper.Agent;
using static Indy.Sdk.Dotnet.Wrapper.AgentObservers;

namespace Indy.Sdk.Dotnet.Test.Wrapper.AnonCredsTests
{

    public abstract class AnonCredsIntegrationTestBase 
    {
        protected static Wallet _commonWallet;
        protected static string _claimDef;

        private static bool _walletOpened = false;

        protected string _masterSecretName = "master_secret_name";
        protected string _issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
        protected string _issuerDid2 = "CnEDk9HrMnmiHXEV1WFgbVCRteYnPqsJwrTdcZaNhFVW";
        protected string _proverDid = "CnEDk9HrMnmiHXEV1WFgbVCRteYnPqsJwrTdcZaNhFVW";
        protected string _claimOfferTemplate = "{{\"issuer_did\":\"{0}\",\"schema_seq_no\":{1}}}";
        protected string _schema = "{\"seqNo\":1,\"data\": {\"name\":\"gvt\",\"version\":\"1.0\",\"keys\":[\"age\",\"sex\",\"height\",\"name\"]}}";
        protected string _claimRequestTemplate =
            "{{\"blinded_ms\":" +
            "{{\"prover_did\":\"CnEDk9HrMnmiHXEV1WFgbVCRteYnPqsJwrTdcZaNhFVW\"," +
            "\"u\":\"54172737564529332710724213139048941083013176891644677117322321823630308734620627329227591845094100636256829761959157314784293939045176621327154990908459072821826818718739696323299787928173535529024556540323709578850706993294234966440826690899266872682790228513973999212370574548239877108511283629423807338632435431097339875665075453785141722989098387895970395982432709011505864533727415552566715069675346220752584449560407261446567731711814188836703337365986725429656195275616846543535707364215498980750860746440672050640048215761507774996460985293327604627646056062013419674090094698841792968543317468164175921100038\"," +
            "\"ur\":null}}," +
            "\"issuer_did\":\"{0}\",\"schema_seq_no\":{1}}}";

        [TestInitialize]
        public void SetUp() 
        {
            InitHelper.Init();
        }

        protected void InitCommonWallet()
        {
            if (_walletOpened)
                return;

            StorageUtils.CleanupStorage();

            var walletName = "anoncredsCommonWallet";

            Wallet.CreateWalletAsync("default", walletName, "default", null, null).Wait();
            _commonWallet = Wallet.OpenWalletAsync(walletName, null, null).Result;

            _claimDef = AnonCreds.IssuerCreateAndStoreClaimDefAsync(_commonWallet, _issuerDid, _schema, null, false).Result;

            AnonCreds.ProverStoreClaimOfferAsync(_commonWallet, string.Format(_claimOfferTemplate, _issuerDid, 1)).Wait();
            AnonCreds.ProverStoreClaimOfferAsync(_commonWallet, string.Format(_claimOfferTemplate, _issuerDid, 2)).Wait();
            AnonCreds.ProverStoreClaimOfferAsync(_commonWallet, string.Format(_claimOfferTemplate, _issuerDid2, 2)).Wait();

            AnonCreds.ProverCreateMasterSecretAsync(_commonWallet, _masterSecretName).Wait();

            var claimOffer = string.Format("{{\"issuer_did\":\"{0}\",\"schema_seq_no\":{1}}}", _issuerDid, 1);

            var claimRequest = AnonCreds.ProverCreateAndStoreClaimReqAsync(_commonWallet, "CnEDk9HrMnmiHXEV1WFgbVCRteYnPqsJwrTdcZaNhFVW", claimOffer, _claimDef, _masterSecretName).Result;

            var claim = "{\"sex\":[\"male\",\"5944657099558967239210949258394887428692050081607692519917050011144233115103\"],\n" +
                    "                 \"name\":[\"Alex\",\"1139481716457488690172217916278103335\"],\n" +
                    "                 \"height\":[\"175\",\"175\"],\n" +
                    "                 \"age\":[\"28\",\"28\"]\n" +
                    "        }";

            var createClaimResult = AnonCreds.IssuerCreateClaimAsync(_commonWallet, claimRequest, claim, -1, -1).Result;
            var claimJson = createClaimResult.ClaimJson;

            AnonCreds.ProverStoreClaimAsync(_commonWallet, claimJson).Wait();

            _walletOpened = true;
        }
    }
}
