import chardet
with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml', 'rb') as f:
    rawdata = f.read(100)
    print(chardet.detect(rawdata))
