#docker run -itd -v /home/efsta/efrDATA:/usr/src/EFR/DATA -p 20011:5618 --restart always efstait/efsta_at:20.04-2.0.10
docker run -itd -p 5618:5618 efstait/efsta_at