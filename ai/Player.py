class Player():
    def __init__(self) -> None:
        self.ppo = [0,  0]               # player position: X Y O
        self.plv = 1                     # player level
        self.pin = [0, 0, 0, 0, 0, 0, 0] # player inventory
        self.tna =  list()               # name of all the teams