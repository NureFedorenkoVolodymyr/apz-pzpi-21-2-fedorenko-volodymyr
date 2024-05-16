import network
import constants
import time

class NetworkService:
    def __init__(self):
        self.wlan = network.WLAN(network.STA_IF)

    def connect_to_network(self):
        self.wlan.active(True)
        if not self.wlan.isconnected():
            print("Connecting to network", end = "")
            self.wlan.connect(constants.WIFI_SSID, constants.WIFI_PASSWORD)
            while not self.wlan.isconnected():
                print(".", end = "")
                time.sleep(0.1)
        print("\nNetwork config:", self.wlan.ifconfig())

    def configure_network(self):
        constants.CLEAR()

        print("0 - Quit")
        print("1 - Change SSID")
        print("2 - Change Password")
        print("3 - Remove Password")
        action = input(">>> ")

        if action == "0":
            return
        elif action == "1":
            self.__change_ssid()
        elif action == "2":
            self.__change_password()
        elif action == "3":
            self.__remove_password()
        else:
            self.configure_network()

    def __change_ssid(self):
        constants.CLEAR()

        ssid = input("New ssid >> ")
        constants.WIFI_SSID = ssid;

        self.configure_network()

    def __change_password(self):
        constants.CLEAR()

        password = input("New password >>> ")
        constants.WIFI_PASSWORD = password;

        self.configure_network()

    def __remove_password(self):
        constants.CLEAR()

        constants.WIFI_PASSWORD = None;

        self.configure_network()
