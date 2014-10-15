user=
read -p "user for ios.analit.net: " user
read -s -p "password: " password
curl -i 'http://'$user:$password'@ios.analit.net/AmpServ/core.asmx/GetPrices?onlyLeader=false&newEar=false&limit=10&selStart=0&rangeField=OriginalName&rangeValue=*&sortField=OriginalName&sortOrder=asc'
