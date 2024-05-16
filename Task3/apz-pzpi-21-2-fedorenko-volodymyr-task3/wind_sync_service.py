import constants
import dht
import time
from machine import Pin
from network_service import NetworkService
from http_service import HttpService
from profile_service import ProfileService

class WindSyncService:
    def __init__(self):
        self.network_service = NetworkService()
        self.http_service = HttpService()
        self.profile_service = ProfileService()
        self.sensor = dht.DHT22(Pin(15))

    def main(self):
        constants.CLEAR()

        print("Welcome to WindSync Smart Device!")
        print("1 - Network settings")
        print("2 - Log in")
        action = input(">>> ")

        constants.CLEAR()

        if action == "1":
            self.network_service.configure_network()
            self.main()
        elif action == "2":
            self.__login()
        else:
            self.main()

    def __login(self):
        self.network_service.connect_to_network()

        email = input("Email >>> ")
        password = input("Password >>> ")
        login_result = self.http_service.login(email, password)

        if login_result:
            print("Login successful!")
            constants.WAIT()
            self.__configuration()
        else:
            print("Login error...")
            constants.WAIT()
            self.main()

    def __configuration(self):
        constants.CLEAR()

        print(f"Turbine ID: {constants.TURBINE_ID}")
        print(f"Logging period: {constants.LOGGING_PERIOD}")

        print("1 - Start logging")
        print("2 - Change turbine ID")
        print("3 - Change logging period")
        action = input(">>> ")

        if action == "1":
            self.__start_logging()
        elif action == "2":
            self.__change_turbine_id()
        elif action == "3":
            self.__change_logging_period()
        else:
            self.__configuration()


    def __change_turbine_id(self):
        constants.CLEAR()

        turbines = self.http_service.get_my_turbines()
        print("My available turbines:")

        for turbine in turbines:
            print(f"ID: {turbine['id']}\tWindFarm ID: {turbine['windFarmId']}")

        turbine_id = input("New turbine ID >>> ")
        self.profile_service.set_turbine_id(turbine_id)

        self.__configuration()

    def __change_logging_period(self):
        constants.CLEAR()

        period = input("New logging period (in seconds) >>> ")
        constants.LOGGING_PERIOD = int(period)
        self.__configuration()

    def __start_logging(self):
        while True:
            constants.CLEAR()

            self.sensor.measure()
            print("Measuring temperature and wind speed...")

            turbine_id = constants.TURBINE_ID
            temperature = self.sensor.temperature()
            wind_speed = self.sensor.humidity() / 2

            print("Data measured:")

            print(f"\ttemperature: {temperature}")
            print(f"\twind speed: {wind_speed}")

            print("Logging data...")
            log_result = self.http_service.add_turbine_data(turbine_id, temperature, wind_speed)
            if log_result:
                print("Data log successful!")
                print(f"Current turbine status: {constants.TURBINE_STATUS[constants.CURRENT_TURBINE_STATUS]}")
            else:
                print("Data log error...")

            time.sleep(constants.LOGGING_PERIOD)