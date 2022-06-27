//Azure IoT Hub + DHT11 + NodeMCU ESP8266 Experiment Done By Prasenjit Saha
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ESP8266HTTPClient.h>

// WiFi settings
const char* ssid = "Jahv";
const char* password = "ja1234567";

const char * headerKeys[] = {"ETag"};
const size_t numberOfHeaders = 1;

//Mensaje
String mensaje = "";

//Azure IoT Hub
const String AzureIoTHubURI = "https://joso-hub.azure-devices.net/devices/esp8266/messages/events?api-version=2020-03-13";
const String AzureIoTHubURIGet = "https://joso-hub.azure-devices.net/devices/esp8266/messages/deviceBound?api-version=2020-03-13";
const String AzureIoTHubURIDel = "https://joso-hub.azure-devices.net/devices/esp8266/messages/deviceBound/{etag}?api-version=2020-03-13";
////openssl s_client -servername joso-hub.azure-devices.net -connect joso-hub.azure-devices.net:443 | openssl x509 -fingerprint -noout
const String AzureIoTHubFingerPrint = "0C:06:77:77:C0:54:D3:65:D9:90:DD:45:48:B9:16:36:3E:C8:30:18";
//az iot hub generate-sas-token --device-id esp8266 --hub-name joso-hub
const String AzureIoTHubAuth = "SharedAccessSignature sr=joso-hub.azure-devices.net%2Fdevices%2Fesp8266&sig=XWH98GPPbCVIySkNUaLptPHn12n2HZm16tIddZC75ks%3D&se=1655560445";

//LEDs
#define LED_NEGATIVO D1
#define LED_NEUTRAL D2
#define LED_POSITIVO D3
#define BUZZER D4

void setup() {
  Serial.begin(115200);
  Serial.println("ESP8266 starting in normal mode");

  // Connect to WiFi
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");

  // Print the IP address
  Serial.println(WiFi.localIP());

  //LEDs
  pinMode(LED_NEGATIVO, OUTPUT);
  pinMode(LED_NEUTRAL, OUTPUT);
  pinMode(LED_POSITIVO, OUTPUT);
}

void loop() {
  String mensaje = RestGetData(AzureIoTHubURIGet, AzureIoTHubFingerPrint, AzureIoTHubAuth);
  Serial.println(mensaje);

  if (mensaje == "Positivo") {
    digitalWrite(LED_POSITIVO , HIGH);
    tone(BUZZER, 440);
    delay(1000);
    digitalWrite(LED_POSITIVO , LOW);
    noTone(BUZZER);
    delay(1000);
  }
  if (mensaje == "Neutral") {
    digitalWrite(LED_NEUTRAL , HIGH);
    tone(BUZZER, 370);
    delay(1000);
    digitalWrite(LED_NEUTRAL , LOW);
    noTone(BUZZER);
    delay(1000);
  }
  if (mensaje == "Negativo") {
    digitalWrite(LED_NEGATIVO , HIGH);
    tone(BUZZER, 277);
    delay(1000);
    digitalWrite(LED_NEGATIVO , LOW);
    noTone(BUZZER);
    delay(1000);
  }
  if (mensaje == "Mixto") {
    digitalWrite(LED_NEGATIVO , HIGH);
    digitalWrite(LED_NEUTRAL , HIGH);
    digitalWrite(LED_POSITIVO , HIGH);
    delay(1000);
    digitalWrite(LED_NEGATIVO , LOW);
    digitalWrite(LED_NEUTRAL , LOW);
    digitalWrite(LED_POSITIVO , LOW);
    delay(1000);
  }
}

// Functions

String RestGetData(String URI, String fingerPrint, String Authorization)
{
  HTTPClient http;
  http.begin(URI, fingerPrint);
  http.addHeader("Authorization", Authorization);
  http.addHeader("Content-Type", "application/atom+xml;type=entry;charset=utf-8");
  http.collectHeaders(headerKeys, numberOfHeaders);
  int httpCode = http.GET();
  String payload;
  if (httpCode < 0)
  {
    Serial.println("RestGetData: Error getting data: " + String(http.errorToString(httpCode).c_str()));
  }
  if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) {
    String eTag = http.header("ETag");
    String newEtag = trimETag(eTag);
    payload = http.getString();
    mensaje = payload;
    String urlNew = AzureIoTHubURIDel;
    urlNew.replace("etag", newEtag);
    int httpdel = RestDelData(urlNew, AzureIoTHubFingerPrint, AzureIoTHubAuth);
    Serial.println(httpdel);
  }
  else {
    payload = String(httpCode);
  }
  http.end();
  return payload;
}


int RestDelData(String URI, String fingerPrint, String Authorization)
{
  HTTPClient http;
  http.begin(URI, fingerPrint);
  http.addHeader("Authorization", Authorization);
  http.addHeader("Content-Type", "application/atom+xml;type=entry;charset=utf-8");
  int httpCode = http.sendRequest("DELETE");;
  if (httpCode < 0)
  {
    Serial.println("RestDelData: Error getting data: " + String(http.errorToString(httpCode).c_str()));
  }
  http.end();
  return httpCode;
}

String trimETag(String value)
{
  String retVal = value;

  if (value.startsWith("\""))
    retVal = value.substring(1);

  if (value.endsWith("\""))
    retVal = retVal.substring(0, retVal.length() - 1);

  return retVal;
}
