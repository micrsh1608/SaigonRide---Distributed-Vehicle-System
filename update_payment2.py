import re

with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml', 'r', encoding='utf-8') as f:
    content = f.read()

content = content.replace('alert("S d vA- Saigon EcoRide khAng  . Vui lAng ch?n phng thcc khAc (MoMo/Zalo/Visa)!");', 'alert(t("alert_insufficient_wallet"));')
content = content.replace('alert("Thanh toAn thAnh cAng! Bn va tA-ch lcy thAm c " + earnedPoints + " i?m xp hng.");', 'alert(t("alert_payment_success").replace("{0}", earnedPoints));')
content = content.replace('alert("L-i t CSDL: " + data.message);', 'alert(t("alert_db_error").replace("{0}", data.message));')
content = content.replace('alert("L-i kt ni Server! Vui lAng th- li.");', 'alert(t("alert_conn_error"));')

# Replace encoded vietnamese alert text from my previous modify
content = content.replace('alert("Số dư ví Saigon EcoRide không đủ. Vui lòng chọn phương thức khác (MoMo/Zalo/Visa)!");', 'alert(t("alert_insufficient_wallet"));')
content = content.replace('alert("Thanh toán thành công! Bạn vừa tích lũy thêm được " + earnedPoints + " điểm xếp hạng.");', 'alert(t("alert_payment_success").replace("{0}", earnedPoints));')
content = content.replace('alert("Lỗi từ CSDL: " + data.message);', 'alert(t("alert_db_error").replace("{0}", data.message));')
content = content.replace('alert("Lỗi kết nối Server! Vui lòng thử lại.");', 'alert(t("alert_conn_error"));')

with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml', 'w', encoding='utf-8') as f:
    f.write(content)
print("Updated Payment.cshtml alerts")
