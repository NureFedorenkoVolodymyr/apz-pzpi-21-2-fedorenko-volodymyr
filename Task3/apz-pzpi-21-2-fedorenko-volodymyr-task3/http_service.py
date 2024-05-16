import urequests
import constants
import time
import json
from profile_service import ProfileService

class HttpService:
    def __init__(self):
        self.profile_service = ProfileService()

    def login(self, email, password):
        url = f"{constants.API_URL}/{constants.LOGIN_ENDPOINT}"

        payload = {
            "email": email,
            "password": password
        }

        headers = {
            "Content-Type": "application/json",
            "ngrok-skip-browser-warning": "true"
            }

        response = urequests.post(url, json=payload, headers=headers)

        if response.status_code == 200:
            self.profile_service.set_access_token(response.text)
            return True
        else:
            return False

    def get_my_turbines(self):
        url = f"{constants.API_URL}/{constants.TURBINES_MY_ENDPOINT}"
        headers = {
            "Authorization": f"Bearer {constants.ACCESS_TOKEN}",
            "ngrok-skip-browser-warning": "true"
            }

        response = urequests.get(url, headers=headers)

        if response.status_code == 200:
            return json.loads(response.text)
        else:
            return []

    def add_turbine_data(self, turbine_id, temperature, wind_speed):
        url = f"{constants.API_URL}/{constants.TURBINE_DATA_ADD_ENDPOINT}"

        temperature = temperature + 273

        payload = {
            "turbineId": turbine_id,
            "airTemperature": temperature,
            "windSpeed": wind_speed
        }

        headers = {
            "Content-Type": "application/json",
            "Authorization": f"Bearer {constants.ACCESS_TOKEN}",
            "ngrok-skip-browser-warning": "true"
            }

        response = urequests.post(url, json=payload, headers=headers)

        if response.status_code == 200:
            self.profile_service.set_turbine_status(int(response.text))
            return True
        else:
            return False

