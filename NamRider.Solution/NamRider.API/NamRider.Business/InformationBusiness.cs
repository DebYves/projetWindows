using NamRider.API.Constants;
using NamRider.API.NamRiderAPI.Persistence;
using NamRider.API.NamRiderDBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamRider.API.NamRider.Service
{
    public class InformationBusiness
    {
        #region Proprities DBAccess and Service Business  methods classes
        private CriticismDrivingDBMethod _criticismDBMethod = new CriticismDrivingDBMethod();
        private DrivingInfoDBMethod _drivingInfoDBMethod = new DrivingInfoDBMethod();
        private ParkingInfoDBMethod _parkingInfoDBMethod = new ParkingInfoDBMethod();
        private EvaluationDBMethod _evaluationDBMethod = new EvaluationDBMethod();
        private CriticismParkingDBMethod _criticismParkingDBMethod = new CriticismParkingDBMethod();
        #endregion

        /// <summary>
        /// Calcul and return value pertinence of information
        /// </summary>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public int CalculValuePertinenceDriving(int idInfo)
        {
            var sumValueCritism = 0;
            var criticisms = _criticismDBMethod.FindByDriving(idInfo);
            foreach (var c in criticisms)
                sumValueCritism += c.Value;

            return (int)(sumValueCritism / (criticisms.Count));
        }

        /// <summary>
        /// Calcul and return value pertinence of information
        /// </summary>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public int CalculValuePertinencePark(int idInfo)
        {
            var sumValueCritism = 0;
            var criticisms = _criticismParkingDBMethod.FindByParking(idInfo);
            foreach (var c in criticisms)
                sumValueCritism += c.Value;

            return (int)(sumValueCritism / (criticisms.Count));
        }

        /// <summary>
        /// Return isPertinence of information
        /// </summary>
        /// <param name="valuePertinence"></param>
        /// <returns></returns>
        public bool IsPertinence(int valuePertinence)
        {
            if (valuePertinence >= ApiProjetConst.Pourcentage)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Calcul and return value pertinence of information
        /// </summary>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public int GetValueSeverityInfo(int idInfo)
        {
            var sumValueEvaluation = 0;
            var evaluations = _evaluationDBMethod.FindByDrivingId(idInfo);
            int nbEvaluation = evaluations.Count;
            foreach (var c in evaluations)
                sumValueEvaluation += c.Value;

            return (int)(sumValueEvaluation / nbEvaluation);
        }

        /// <summary>
        /// Return latitude longitude string format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetDisplayFormatLatitudeLongitude(decimal value)
        {
            int partDegre = (int)value;
            double valMinute = (double)(value - partDegre) * ApiProjetConst.ConstantConvertPoint;
            int partMinute = (int)valMinute;
            double partSecond = (double)(valMinute - partMinute) * ApiProjetConst.ConstantConvertPoint;
            double partSecondArrond = Math.Round(partSecond, ApiProjetConst.ConstantSecondConvertPoint);
            return partDegre + "°" + partMinute + "'" + partSecondArrond + "";
        }
        /// <summary>
        ///  Return list type park
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<string> TransformFilterModelToList(ParkingInfoFilter model)
        {
            List<string> listFilter = new List<string>();
            listFilter.Add(model.AlternancyZone);
            listFilter.Add(model.DisckZone);
            listFilter.Add(model.FreeZone);
            listFilter.Add(model.PayZone);
            listFilter.Add(model.SubcriptionZone);

            return listFilter;
        }

        /// <summary>
        /// Return reported out date value for driving
        /// </summary>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public bool CalculReportedDateDriving(int idInfo)
        {
            var alert = _drivingInfoDBMethod.GetNumberAlert(idInfo);
            if (alert >= ApiProjetConst.ReporteOutdDateTaux)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Return reported out date value for Park
        /// </summary>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public bool CalculReportedDatePark(int idInfo)
        {
            var alert = _parkingInfoDBMethod.GetNumberAlert(idInfo);
            if (alert >= ApiProjetConst.ReporteOutdDateTaux)
                return true;
            else
                return false;
        }
    }
}