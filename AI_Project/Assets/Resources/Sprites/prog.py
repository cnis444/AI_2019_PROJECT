# coding: utf-8

import sys
import time
import os

if __name__ == "__main__":
    print("start")
    path = os.path.join("Assets","Ressources", "test.txt")
    print(path)
    try :
        mon_fichier = open(path, "w")
    except:
        print("erreur : ", sys.exc_info()[0])
    mon_fichier.write("aaaaa")
    mon_fichier.close()
    print("end")
