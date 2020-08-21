# Social Networking Kata

Il codice in questo progetto è stato realizzato come test attitudinale per Claranet sulla base dei requisiti disponibili al seguente [link](https://github.com/xpeppers/social_networking_kata).

## Descrizione

Il progetto è stato realizzato con Visual Studio 2019 e .NET Core 3.1. Al suo interno fa uso della libreria [Lamar](https://jasperfx.github.io/lamar/documentation/ioc/) come dependency container allo scopo di migliorare la testabilità del codice.

La soluzione è divisa in due progetti:

### Claranet.SocialNetworkingKata

Si tratta del progetto Console la cui compilazione genera l'applicazione eseguibile. 

### Claranet.SocialNetworkingKata.Tests

Si tratta di un set minimale di unit tests usati allo scopo di validare il funzionamento di alcuni componenti critici anche mediante TDD. In particolare vi si trova anche un test di integrazione (RequirementsTests.cs) che riprende i requisiti espressi e li valida mediante un test di integrazione.

## Avvio del progetto

In questo paragrafo si illustrano le medalità di avvio del progetto

All'avvio l'applicazione chiede alcune informazioni:

1) Provider da utilizzare: sono disponibili i provider di accesso ai dati sqllite e memory. Il provider sqlite è persistente pertanto ad ogni avvio dell'applicazione i post precedenti verranno mantenuti. Il provider memory invece viene azzerato ad ogni riavvio. Esso ha specificamente scopi di sviluppo/test.
2) Modalità di debug: impostando "yes" la console emetterà alcuni messaggi aggiuntivi rispetto ai requisiti. Per essere aderenti ai requisiti impostare "no" (valore di default)
3) Azzeramento del database: nel caso in cui si scelga il provider sqllite l'applicazione chiede se si vuole cancellare un precedente database nel caso esso sia presente.

Impostando i valori di default (invio ripetuto) si otterrà il comportamento persistente più aderente ai requisiti espressi.

### Avvio del progetto da Visual Studio 2019 (debug)

1) aprire il progetto con Visual Studio 2019
2) impostare Claranet.SocialNetworkingKata come progetto di avvio
3) Premere F5

### Avvio del progetto da console (publish)

1) aprire una powershell o command prompt
2) posizionarsi nella cartella dove è situato il file Claranet.SocialNetworkingKata.sln
3) eseguire il comando run.bat - il comando alla prima esecuzione compila la soluzione e la pubblica in un folder "build". In seguito avvia l'applicazione

