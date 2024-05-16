import constants

class ProfileService:
    def __init__(self):
        return

    def set_access_token(self, token):
        constants.ACCESS_TOKEN = token

    def set_turbine_id(self, turbine_id):
        constants.TURBINE_ID = turbine_id

    def set_turbine_status(self, turbine_status):
        constants.CURRENT_TURBINE_STATUS = turbine_status