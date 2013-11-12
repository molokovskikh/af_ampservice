user=
read -p "user for ios.analit.net: " user
read -s -p "password: " password
curl -i http://$user:$password@ios.analit.net/AmpServ/
