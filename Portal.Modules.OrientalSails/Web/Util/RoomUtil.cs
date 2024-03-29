using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Portal.Modules.OrientalSails.Domain;

namespace Portal.Modules.OrientalSails.Web.Util
{
    public class RoomUtil
    {
        private readonly SailsModule _module;
        private int _customerCount; // Số khách
        protected Dictionary<int, IList> _rooms; // Map giữa tàu và danh sách các loại phòng
        protected Dictionary<int, Dictionary<string, int>> _roomCountMap; // Map giữa tàu và map giữa loại phòng và số lượng
        public Dictionary<int, Dictionary<string, int>> RoomCountMap
        {
            get
            {
                if (_roomCountMap == null)
                {
                    _roomCountMap = new Dictionary<int, Dictionary<string, int>>();
                }
                return _roomCountMap;
            }
        }

        protected Dictionary<int, Dictionary<string, int>> _bookedRoom; // Map giữa tàu và map giữa loại phòng và số lượng đã book
        protected Dictionary<int, Dictionary<string, int>> BookedRoom
        {
            get
            {
                if (_bookedRoom == null)
                {
                    _bookedRoom = new Dictionary<int, Dictionary<string, int>>();
                }
                return _bookedRoom;
            }
        }

        private IList _bookings;

        public IList Bookings
        {
            get { return _bookings; }
            set { _bookings = value; }
        }

        public RoomUtil(SailsModule module)
        {
            _module = module;
            _rooms = new Dictionary<int, IList>();
        }

        /// <summary>
        /// Lấy danh sách các loại phòng khác nhau ở trên tàu
        /// </summary>
        /// <param name="cruise"></param>
        /// <returns></returns>
        public IList Rooms(Cruise cruise)
        {
            if (!_rooms.ContainsKey(cruise.Id))
            {
                IList rooms = _module.RoomGetAll(cruise);
                IList roomtypes = new ArrayList();

                // Kiểm tra đã có bảng map tàu/ loại phòng trong từ điển hay chưa, nếu chưa thì tạo mới
                Dictionary<string, int> roomMap = new Dictionary<string, int>();
                if (RoomCountMap.ContainsKey(cruise.Id))
                {
                    RoomCountMap[cruise.Id] = roomMap;
                }
                else
                {
                    RoomCountMap.Add(cruise.Id, roomMap);
                }

                foreach (Room room in rooms)
                {
                    if (!roomMap.ContainsKey(string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id)))
                    {
                        roomMap.Add(string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id), 1);
                        roomtypes.Add(room); // Nếu là loại chưa có trong từ điển thì thêm vào danh sách loại
                    }
                    else
                    {
                        roomMap[string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id)] += 1;
                    }
                }

                _rooms.Add(cruise.Id, roomtypes);
            }
            return _rooms[cruise.Id];
        }

        /// <summary>
        /// Tính số phòng đã booking
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetRoomCount(Cruise cruise)
        {
            Dictionary<string, int> currentRoomMap = new Dictionary<string, int>();
            foreach (Booking booking in _bookings)
            {
                if (booking.Cruise != cruise)
                {
                    continue;
                }
                // Tính số phòng chiếm/phòng trống của từng loại
                foreach (BookingRoom room in booking.BookingRooms)
                {
                    string key = string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id);
                    int add;
                    if (room.RoomType.IsShared)
                    {
                        add = room.VirtualAdult;
                    }
                    else
                    {
                        add = 1;
                    }
                    if (currentRoomMap.ContainsKey(key))
                    {
                        currentRoomMap[key] += add;
                    }
                    else
                    {
                        currentRoomMap.Add(key, add);
                    }
                }
            }
            return currentRoomMap;
        }

        public int BookingCount()
        {
            if (_bookings != null)
            {
                return _bookings.Count;
            }
            return 0;
        }

        public int CustomerCount()
        {
            if (_customerCount == 0)
            {
                foreach (Booking booking in _bookings)
                {
                    _customerCount += booking.Pax;
                }
            }
            return _customerCount;
        }

        public int BookingCount(SailsTrip trip)
        {
            int result = 0;
            if (_bookings != null)
            {
                foreach (Booking booking in _bookings)
                {
                    if (booking.Trip == trip)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public int CustomerCount(SailsTrip trip)
        {
            int result = 0;
            foreach (Booking booking in _bookings)
            {
                if (booking.Trip == trip)
                {
                    result += booking.Pax;
                }                
            }
            return result;
        }
    }
}
