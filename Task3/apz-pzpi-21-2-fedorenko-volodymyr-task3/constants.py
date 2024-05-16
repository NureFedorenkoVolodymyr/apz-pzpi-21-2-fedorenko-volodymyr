import time

WIFI_SSID = "Wokwi-GUEST"
WIFI_PASSWORD = ""

LOGGING_PERIOD = 20

API_URL = "http://7.tcp.eu.ngrok.io:19268/api"

LOGIN_ENDPOINT = "auth/login"
TURBINES_MY_ENDPOINT = "turbines/my"
TURBINE_DATA_ADD_ENDPOINT = "turbinedata"

ACCESS_TOKEN = ""
TURBINE_ID = 1
CURRENT_TURBINE_STATUS = 0

TURBINE_STATUS = {
  0: "None",
  1: "Operational",
  2: "Fault",
  3: "Idle",
  4: "UnderMaintenance"
}

CLEAR = lambda: print("\033[H\033[J", end="")
WAIT = lambda: time.sleep(1)