using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulCsharpCommonsUtils.lang;
using static UsefulCsharpCommonsUtils.webfunction.youtrackv4.YoutrackFieldAttribute;

namespace UsefulCsharpCommonsUtils.webfunction.youtrackv4
{
    public class YoutrackObject 
    {
        private string _id;
        private string _project;
        private string _subsystem;
        private string _fixVersion;
        private string _summary;
        private string _typeYt;
        private string _state;
        private string _sheet;
        private string _affectation;
        private string _demandeur;
        private string _specialId;

        public List<string> PropertyUpdated { get; } = new List<string>();


        [YoutrackField("id", YoutrackIssueElt.ATTR)]
        public string Id
        {
            get => _id;
            set
            {
                if (!LangUtils.IsEqWithNull(_id, value)) PropertyUpdated.Add(nameof(Id));
                _id = value;
            }
        }

        [YoutrackField("projectShortName", YoutrackIssueElt.FIELD)]
        public string Project
        {
            get => _project;
            set
            {
                if (!LangUtils.IsEqWithNull(_project, value)) PropertyUpdated.Add(nameof(Project));
                _project = value;
            }
        }

        [YoutrackField("Subsystem", YoutrackIssueElt.CFIELD)]
        public string Subsystem
        {
            get => _subsystem;
            set
            {
                if (!LangUtils.IsEqWithNull(_subsystem, value)) PropertyUpdated.Add(nameof(Subsystem));
                _subsystem = value;
            }
        }

        [YoutrackField("Fix versions", YoutrackIssueElt.CFIELD)]
        public string FixVersion
        {
            get => _fixVersion;
            set
            {
                if (!LangUtils.IsEqWithNull(_fixVersion, value)) PropertyUpdated.Add(nameof(FixVersion));
                _fixVersion = value;
            }
        }

        [YoutrackField("summary", YoutrackIssueElt.CFIELD)]
        public string Summary
        {
            get => _summary;
            set
            {
                if (!LangUtils.IsEqWithNull(_summary, value)) PropertyUpdated.Add(nameof(Summary));
                _summary = value;
            }
        }

        [YoutrackField("Type", YoutrackIssueElt.CFIELD)]
        public string TypeYt
        {
            get => _typeYt;
            set
            {
                if (!LangUtils.IsEqWithNull(_typeYt, value)) PropertyUpdated.Add(nameof(TypeYt));
                _typeYt = value;
            }
        }

        [YoutrackField("State", YoutrackIssueElt.CFIELD)]
        public string State
        {
            get => _state;
            set
            {
                if (!LangUtils.IsEqWithNull(_state, value)) PropertyUpdated.Add(nameof(State));
                _state = value;
            }
        }

        [YoutrackField("Sheet", YoutrackIssueElt.CFIELD)]
        public string Sheet
        {
            get => _sheet;
            set
            {
                if (!LangUtils.IsEqWithNull(_sheet, value)) PropertyUpdated.Add(nameof(Sheet));
                _sheet = value;
            }
        }

        [YoutrackField("Affectation", YoutrackIssueElt.CFIELD)]
        public string Affectation
        {
            get => _affectation;
            set
            {
                if (!LangUtils.IsEqWithNull(_affectation, value)) PropertyUpdated.Add(nameof(Affectation));
                _affectation = value;
            }
        }

        [YoutrackField("Demandeur", YoutrackIssueElt.CFIELD)]
        public string Demandeur
        {
            get => _demandeur;
            set
            {
                if (!LangUtils.IsEqWithNull(_demandeur, value)) PropertyUpdated.Add(nameof(Demandeur));
                _demandeur = value;
            }
        }

        public string SpecialId
        {
            get => _specialId;
            internal set => _specialId = value;
        }
        //public string TypeSubsystem { get; set; }



    }
}
