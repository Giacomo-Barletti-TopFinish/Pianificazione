using PianificazioneService.Properties;
using ScheduleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace PianificazioneService
{
    public class WindowsService
    {
        private LogWriter _log = HostLogger.Get<WindowsService>();
        private Timer _timer;
        public void Start()
        {
            try
            {
                string messaggio = string.Format("Timer impostato a {0} minuti", Settings.Default.TimerPeriod);
                _timer = new Timer(TimerCallBack, null, 1L, Settings.Default.TimerPeriod * 60 * 1000);
                _log.Info("Servizio Pianificazione avviato");
                _log.Info(messaggio);
                Console.WriteLine(messaggio);
            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di start del servizio", ex);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine(ex.Source);
                Console.WriteLine("Errore in fase di start del servizio");
                Console.WriteLine(sb.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                _timer.Dispose();
                _log.Info("Servizio Pianificazione fermato");
            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di stop del servizio", ex);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine(ex.Source);
                Console.WriteLine("Errore in fase di stop del servizio");
                Console.WriteLine(sb.ToString());

            }
        }

        private void TimerCallBack(Object stateInfo)
        {
            Console.WriteLine("** Timercallback");
            try
            {
#if DEBUG
                Pianificazione.Service.PianificazioneService p = new Pianificazione.Service.PianificazioneService();
                p.TrovaOCPerFasiAccantonate();
                p.CreaPianificazioneSuBaseODL();
#endif
                ScheduleService.ScheduleService sCheduler = new ScheduleService.ScheduleService();
                ScheduleDS.MONITOR_SCHEDULERRow schedulazione;

                if (sCheduler.VerificaEsecuzione("PIANIFICAZIONE_1", out schedulazione))
                {
                    Console.WriteLine("PIANIFICAZIONE_1");
                    Pianificazione.Service.PianificazioneService pianificazione = new Pianificazione.Service.PianificazioneService();
                    //   pianificazione.CreaPianificazione();
                    pianificazione.TrovaOCPerFasiAccantonate();
                    pianificazione.CreaPianificazioneSuBaseODL();
                    sCheduler.AggiornaSchedulazione(schedulazione);
                }

                if (sCheduler.VerificaEsecuzione("PIANIFICAZIONE_2", out schedulazione))
                {
                    Console.WriteLine("PIANIFICAZIONE_2");
                    Pianificazione.Service.PianificazioneService pianificazione = new Pianificazione.Service.PianificazioneService();
                    //     pianificazione.CreaPianificazione();
                    pianificazione.TrovaOCPerFasiAccantonate();
                    pianificazione.CreaPianificazioneSuBaseODL();
                    sCheduler.AggiornaSchedulazione(schedulazione);
                }

                if (sCheduler.VerificaEsecuzione("PIANIFICAZIONE_3", out schedulazione))
                {
                    Console.WriteLine("PIANIFICAZIONE_3");
                    Pianificazione.Service.PianificazioneService pianificazione = new Pianificazione.Service.PianificazioneService();
                    // pianificazione.CreaPianificazione();
                    pianificazione.TrovaOCPerFasiAccantonate();
                    pianificazione.CreaPianificazioneSuBaseODL();
                    sCheduler.AggiornaSchedulazione(schedulazione);
                }
                Console.WriteLine("** Timercallback END");
                if (sCheduler.VerificaEsecuzione("PIANIFICAZIONE_4", out schedulazione))
                {
                    Console.WriteLine("PIANIFICAZIONE_4");
                    Pianificazione.Service.PianificazioneService pianificazione = new Pianificazione.Service.PianificazioneService();
                    //pianificazione.CreaPianificazione();
                    pianificazione.TrovaOCPerFasiAccantonate();
                    pianificazione.CreaPianificazioneSuBaseODL();
                    sCheduler.AggiornaSchedulazione(schedulazione);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Eccezione");
                StringBuilder sb = new StringBuilder();
                _log.Error("Errore Servizio Pianificazione", ex);
                while (ex.InnerException != null)
                {
                    _log.Error("--- INNER EXCEPTION", ex);
                    ex = ex.InnerException;
                    sb.AppendLine(ex.Message);
                    sb.AppendLine(ex.StackTrace);
                    sb.AppendLine(ex.Source);
                }
                Console.WriteLine("Errore Servizio Pianificazione");
                Console.WriteLine(sb.ToString());

            }
            finally
            {
            }
        }
    }
}
